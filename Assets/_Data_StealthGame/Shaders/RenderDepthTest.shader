Shader "StealthGame/RenderDepthTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" "IgnoreProjector" = "True" "renderPipeline" = "UniversalPipeline" }
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

            //#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl" // assign some default properties for CBuffer
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl" // declaration of _CameraDepthTexture
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl" // VertexPositionInput, etc.
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl" // LinearEyeDepth(), etc.

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
                float4 projectedPosition : TEXCOORD2; // can be used as screen space position
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            float4 _MainTex_ST;
            SAMPLER(sampler_linear_repeat);

            

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

                output.vertex = vertexInput.positionCS;
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.positionWS = vertexInput.positionWS;
                output.projectedPosition = vertexInput.positionNDC;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                half2 uv = input.uv;

                // sample depth
                // // _ZBufferParams = { (f-n)/n, 1, (f-n)/n*f, 1/f }
                // can sample depth in screen space coordinates
                // the same as the "Raw" option of the ScreenDepth node of the Shader Graph
                float sceneZ = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(input.projectedPosition.xy / input.projectedPosition.w)).r;

                // the same as the "Eye" option of the ScreenDepth node of the Shader Graph
                //float sceneZ = LinearEyeDepth(SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(input.projectedPosition.xy / input.projectedPosition.w)).r, _ZBufferParams);
                
                // the same as the "Linear01" option of the ScreenDepth node of the Shader Graph
                //float sceneZ = Linear01Depth(SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(input.projectedPosition.xy / input.projectedPosition.w)).r, _ZBufferParams);
                
                // get depth of "this" object
                //float thisZ = LinearEyeDepth(input.projectedPosition.z / input.projectedPosition.w, _ZBufferParams);
                
                sceneZ = pow(sceneZ, 0.3);
                float4 texColor = float4(sceneZ, 0, 0, 1);
                return texColor;
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/InternalErrorShader"
}
