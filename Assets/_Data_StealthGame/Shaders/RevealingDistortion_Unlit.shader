Shader "StealthGame/RevealingDistortion_Unlit"
{
    Properties
    {
        _Albedo ("Albedo", 2D) = "white" {}
        _Normal("Normal", 2D) = "bump"{}
        _Color("BaseColor", Color) = (1,1,1,1)
        [HDR]_EmissionColor("Emission Color", Color) = (0,0,0,0)
        _Feather("Feather", Range(0,1)) = 0.1
        _RevealArea("RevealArea", Vector) = (0,0,0,0)
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

            #include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
            #include "RevealingFunctions.hlsl"
            #include "ShaderCalculationHelper.hlsl"

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
                float3 worldPos : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_Albedo);
            float4 _Albedo_ST;
            TEXTURE2D(_Normal);
            half4 _Color;
            half4 _EmissionColor;
            half _Feather;
            float4 _RevealArea;
            TEXTURE2D(_NoiseTex);
            TEXTURE2D(_DistortionTex);
            float4 _NoiseTilingOffset1;
            float4 _NoiseTilingOffset2;
            SAMPLER(sampler_linear_repeat);


            Varyings vert (Attributes input)
            {
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

                output.vertex = vertexInput.positionCS;
                output.uv = TRANSFORM_TEX(input.uv, _Albedo);
                output.worldPos = vertexInput.positionWS;
                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                
                // sample the texture
                half2 uv = input.uv;
                half4 texColor = SAMPLE_TEXTURE2D(_Albedo, sampler_linear_repeat, uv);
                half3 color = texColor.rgb *_Color.rgb;
                half alpha = texColor.z * _Color.a;

                // calculate distance from revealing center
                float distortion = SAMPLE_TEXTURE2D(_DistortionTex, sampler_linear_repeat, uv).r;


                alpha *= GetFadingBorder(fitRange(distortion, 0, 1, -0.2, 0.2) + distance(input.worldPos, _RevealArea.xyz), _RevealArea, _Feather);

                AlphaDiscard(alpha, _Cutoff);

#ifdef _ALPHAPREMULTIPLY_ON
                color *= alpha;
#endif

                return half4(color, alpha);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/InternalErrorShader"
}
