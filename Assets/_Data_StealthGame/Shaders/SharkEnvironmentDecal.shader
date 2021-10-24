Shader "StealthGame/SharkEnvironmentDecal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _VertexOffset ("Vertex Offset", Float) = 0.5
        [HDR]_BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        [HDR]_SecondaryColor("Secondary Color", Color) = (1, 1, 1, 1)
        _FarTintColor("Far Tint Color", Color) = (0.5, 0, 1, 1)
        [Header(Revealing effect)]
        _RevealArea("Reveal Area", Vector) = (0, 0, 0, 0)
        _Feather("Feather", Float) = 0.2
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
                float3 positionVS : TEXCOORD5;
                float3 positionWSOriginal : TEXCOORD6;
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
            half3 _SecondaryColor;
            half3 _FarTintColor;
            float4 _RevealArea;
            float _Feather;
            CBUFFER_END


            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                // world space position of the origianl size of mesh
                output.positionWSOriginal = mul(unity_ObjectToWorld, input.positionOS).xyz;

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

                // view space position
                output.positionVS = vertexInput.positionVS;

                return output;
            }

            // get fading border for revealing effect
            float GetFadingBorder(float distanceFromCenterPoint, float4 revealArea, float feather)
            {
                return smoothstep(revealArea.w, revealArea.w - feather, distanceFromCenterPoint);
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
                float3 worldSpacePosBehind = mul(unity_ObjectToWorld, objectSpacePosBehind).xyz;

                // project a texture cylindrically; the origin of the cylinder is the pivot of the mesh
                float2 cylindricalAngle = (worldSpacePosBehind - mul(unity_ObjectToWorld, float3(0, 0, 0)).xyz).xz;
                cylindricalAngle = normalize(cylindricalAngle);
                float2 decalUv1 = float2(fitRange(atan2(cylindricalAngle.x, cylindricalAngle.y), -3.14, 3.14, 0, 1), worldSpacePosBehind.y) + 0.5;

                // apply tiling and offset on uv
                decalUv1 = decalUv1 * _MainTex_ST.xy + _MainTex_ST.zw;

                // sample texture by object space opaque mesh position
                float4 texPattern = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, decalUv1);
                float dist = length(objectSpacePosBehind);
                float feather = 0.4;
                float affectArea = smoothstep(0, feather, length(input.positionVS) - abs(sceneDepth));

                float noise1 = ValueNoise(float3(floor(worldSpacePosBehind.x * 0.01) * 1000, worldSpacePosBehind.y * 40, floor(worldSpacePosBehind.z * 0.01) * 1000) + _Time.zxy);
                float noise2 = ValueNoise( float3(floor(worldSpacePosBehind.x * 0.01) * 2000, worldSpacePosBehind.y * 60, floor(worldSpacePosBehind.z * 0.01) * 2000) + _Time.yzx);
                float2 noiseGB = float2(pow(noise1, 5), noise2 * (1- noise1));

                // define fading area
                float revealDist = distance(_RevealArea.xyz, input.positionWSOriginal);
                float fadingArea = GetFadingBorder(revealDist, _RevealArea, _Feather);
                affectArea *= fadingArea;

                // clip completely transparent area
                clip(affectArea);

                // sine wave pattern
                float wavePattern = smoothstep( -1, 1, sin(dist * 5 + _Time.z));
                //wavePattern = pow(texPattern.r, lerp(1, 5, wavePattern));
                clip(wavePattern);

                // fade for near area
                float nearFade = smoothstep(length(viewVector * sceneDepth), length(viewVector * sceneDepth) + feather, length(input.viewDirectionOS.xyz));
                nearFade = pow(nearFade, 2);

                // fade for far area; can be used for edge of the affect area
                float farFadeDist = 1;
                float farFade = smoothstep(length(viewVector * sceneDepth) + farFadeDist + feather, length(viewVector * sceneDepth) + farFadeDist, length(input.viewDirectionOS.xyz));

                half4 color = half4(_BaseColor, (half) affectArea);
                color.rgb = lerp(color.rgb, _SecondaryColor, noiseGB.x);
                color.rgb = GetFarTintColor(color.rgb, _FarTintColor, worldSpacePosBehind);

                // apply scanlines
                float horizontalScanlines = pow(sin((input.positionWS.y + _Time.x) * 300) * 0.5 + 1, max(0.5, 3 * (1 - affectArea)));

                color.a *= lerp(horizontalScanlines, 1, affectArea);
                
                // apply glitch
                color.a *= DropPixel(texPattern.r, _Time.y, 0.1);
                //color.rgb += DropPixel(texPattern.r, _Time.y, 0.1);
                
                clip(color.a);
                return color;
            }
        ENDHLSL
        }
    }
    FallBack "Hidden/InternalErrorShader"
}
