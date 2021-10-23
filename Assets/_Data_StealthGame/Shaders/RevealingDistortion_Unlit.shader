Shader "StealthGame/RevealingDistortion_Unlit"
{
    Properties
    {
        _Albedo ("Albedo", 2D) = "white" {}
        _Normal("Normal", 2D) = "bump"{}
        _Color("BaseColor", Color) = (1,1,1,1)
        [HDR]_EmissionColor("Emission Color", Color) = (0,0,0,0)
        _Feather("Feather", Range(0,1)) = 0.1
        _NoiseTex("NoiseTex", 2D) = "white"{}
        _DistortionTex("DistortionTex", 2D) = "grey"{}
        _NoiseTilingOffset1("NoiseTilingOffset1", Vector) = (1,1,0,0)
        _NoiseTilingOffset2("NoiseTilingOffset2", Vector) = (1,1,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="Transparent" "IgnoreProjector" = "True" "renderPipeline"="UniversalPipeline" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back
        LOD 100

        Pass
        {
            Name "Unlit"
            
            HLSLPROGRAM
            
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON

            #pragma multi_compile_instancing

            #pragma target 5.0

            #include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl" // assign some default properties for CBuffer
            #include "RevealingFunctions.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 positionWS : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_Albedo);
            float4 _Albedo_ST;
            TEXTURE2D(_Normal);
            half4 _Color;
            half4 _EmissionColor;
            half _Feather;
            TEXTURE2D(_NoiseTex);
            TEXTURE2D(_DistortionTex);
            float4 _NoiseTilingOffset1;
            float4 _NoiseTilingOffset2;
            SAMPLER(sampler_linear_repeat);

            // single reveal area structure
            struct RevealArea
            {
                int _id; // identify areas from c# scripts
                float3 _origin; // origin of the reveal area
                float _range; // current radious of the reveal area
                float _alpha; // phase of fading out of the reveal area
            };

            // buffer for revealing area
            StructuredBuffer<RevealArea> _revealAreaBuffer;

            Varyings vert (Attributes input)
            {
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

                output.vertex = vertexInput.positionCS;
                output.uv = TRANSFORM_TEX(input.uv, _Albedo);
                output.positionWS = vertexInput.positionWS;
                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                
                half2 uv = input.uv;

                // sample the texture
                half4 texColor = SAMPLE_TEXTURE2D(_Albedo, sampler_linear_repeat, uv);
                half3 color = texColor.rgb * _Color.rgb;
                half alpha = texColor.z * _Color.a;

                // uv scroll
                float2 scrolledUv = input.uv + float2(_Time.x, 0);

                // calculate distance from revealing center
                float distortion = SAMPLE_TEXTURE2D(_DistortionTex, sampler_linear_repeat, scrolledUv).r;

                // apply noise pattern with doubled texture sampling
                float doubledNoise = SampleTextureWidhDoubledUv(_NoiseTilingOffset1, _NoiseTilingOffset2, input.uv, _NoiseTex, sampler_linear_repeat).r;

                // calculate revealing area
                float distortedDistanceFromCenterPoint = 0;
                float fadeAlpha = 0;
                float featherAroundFadingBorder = 0;
                float emissionLerpFactor = 0;
                float4 revealArea;
                uint length = 0;
                uint stride = 0;
                _revealAreaBuffer.GetDimensions(length, stride);
                if (length > 0)
                {
                    [unroll(64)]
                    for (uint i = 0; i < length; i++)
                    {
                        if (_revealAreaBuffer[i]._id < 0) { continue; }
                        // pack relative reveal area in the structured buffer
                        revealArea = float4(_revealAreaBuffer[i]._origin, _revealAreaBuffer[i]._range);

                        // calculate total revealing fade
                        distortedDistanceFromCenterPoint = fitRange(distortion, 0, 1, -0.2, 0.2) + distance(input.positionWS, _revealAreaBuffer[i]._origin);
                        fadeAlpha += GetFadingBorder(distortedDistanceFromCenterPoint, revealArea, _Feather) * _revealAreaBuffer[i]._alpha;
                    }

                    // clip alpha
                    fadeAlpha = saturate(fadeAlpha);

                    // calculate feather for emission
                    featherAroundFadingBorder = 1 - fadeAlpha;
                    emissionLerpFactor = saturate(saturate(pow(doubledNoise.r * 2, 4)) + featherAroundFadingBorder);
                }

                // integrate alpha
                alpha *= fadeAlpha;
                clip(alpha - _Cutoff);

                color.rgb = lerp(color.rgb, _EmissionColor.rgb, emissionLerpFactor);
                return half4(color, alpha);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/InternalErrorShader"
}
