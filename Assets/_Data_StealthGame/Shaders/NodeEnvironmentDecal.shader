Shader "StealthGame/NodeEnvironmentDecal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _VertexOffset ("Vertex Offset", Float) = 0.5
        [HDR]_BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        [HDR]_FarTintColor("Far Tint Color", Color) = (0.5, 0, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "IgnoreProjector" = "True" "renderPipeline" = "UniversalPipeline" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Front ZWrite On /*Ztest GEqual*/
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
            #include "MainObjectFunctions.hlsl"

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
                float4 viewDirectionOS : TEXCOORD3;
                float3 cameraPositionOS : TEXCOORD4;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            // cbuffer contains exposed properties
            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            half _VertexOffset;
            half3 _BaseColor;
            half3 _FarTintColor;
            CBUFFER_END


            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                // offset vertex position
                input.positionOS.xyz += (input.positionOS.xyz / length(input.positionOS.xyz)) * _VertexOffset;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

                output.vertex = vertexInput.positionCS;
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.positionWS = vertexInput.positionWS;

                // screen space position
                output.projectedPosition = vertexInput.positionNDC;

                // get view direction("vertex to camera" vector) in object space
                float3 viewDir = vertexInput.positionVS;
                viewDir *= -1;

                // assuming to use unity's cube and quad
                float4x4 viewToObjectMatrix = mul(UNITY_MATRIX_I_M, UNITY_MATRIX_I_V);
                output.viewDirectionOS.xyz = mul((float3x3)viewToObjectMatrix, viewDir);
                output.viewDirectionOS.w = vertexInput.positionVS.z;

                // convert camera position from view space to object space in vertex shader
                output.cameraPositionOS = mul(viewToObjectMatrix, float4(0, 0, 0, 1)).xyz;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                half2 uv = input.uv;

                // sample depth
                // mask with depth, so that exit calculation if there's no mesh behind
                float sceneDepth01 = Linear01Depth(SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(input.projectedPosition.xy / input.projectedPosition.w)).r, _ZBufferParams);
                clip(step(0.01, 1 - sceneDepth01) - 0.01);

                // linear eye depth is used to get object space position of the mesh behind
                float sceneDepth = LinearEyeDepth(SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(input.projectedPosition.xy / input.projectedPosition.w)).r, _ZBufferParams);

                // input.viewDirectionOS.xyz is NOT a unit vector, but its z is 1
                float3 viewVector = input.viewDirectionOS.xyz / input.viewDirectionOS.w;
                float3 objectSpacePosBehind = input.cameraPositionOS + viewVector * sceneDepth;

                // the object space coordinates of unity's cube or quad are [-1, 1]
                // so, they should be converted to[0, 1] to sample textures appropriately
                /*
                float2 decalUv1 = objectSpacePosBehind.xy + 0.5;
                float2 decalUv2 = objectSpacePosBehind.yz + 0.5;
                float2 decalUv3 = objectSpacePosBehind.xz + 0.5;

                // apply tiling and offset on uv
                decalUv1 = decalUv1 * _MainTex_ST.xy + _MainTex_ST.zw;
                decalUv2 = decalUv2 * _MainTex_ST.xy + _MainTex_ST.zw;
                decalUv3 = decalUv3 * _MainTex_ST.xy + _MainTex_ST.zw;

                // sample texture by object space opaque mesh position
                float4 texPattern = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, decalUv1);
                texPattern += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, decalUv2);
                texPattern += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, decalUv3);
                texPattern /= 3;
                */
                float4 texPattern = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);

                float dist_thisMesh = length(mul(unity_WorldToObject, input.positionWS).xyz);
                float dist = length(objectSpacePosBehind);
                float feather = 0.4;
                float affectArea = smoothstep(dist_thisMesh, dist_thisMesh - feather, dist);
                
                // clip by a range from the origin of the mesh
                clip(_VertexOffset - dist);

                // sine wave pattern
                float wavePattern = smoothstep( -1, 1, sin(dist * 5 + _Time.z));
                wavePattern = pow(texPattern.r, lerp(1, 5, wavePattern));

                // fade for near area
                affectArea *= smoothstep(length(viewVector * sceneDepth), length(viewVector * sceneDepth) + feather, length(input.viewDirectionOS.xyz));

                // fade for far area
                //float farFadeDist = 1;
                //affectArea *= smoothstep(length(viewVector * sceneDepth) + farFadeDist + feather, length(viewVector * sceneDepth) + farFadeDist, length(input.viewDirectionOS.xyz));
                float4 color = float4(_BaseColor, affectArea * wavePattern);
                color.rgb = GetFarTintColor(color.rgb, _FarTintColor, input.positionWS);
                clip(color.a);
                return color;
            }
        ENDHLSL
        }
    }
    FallBack "Hidden/InternalErrorShader"
}
