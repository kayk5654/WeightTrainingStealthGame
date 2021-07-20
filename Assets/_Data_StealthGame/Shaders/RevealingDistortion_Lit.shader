Shader "StealthGame/RevealingDistortion_Lit"
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
            Name "ForwardLit"
            Tags {"LightMode"="UnivresalForward"}
            
            HLSLPROGRAM
            
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag

            // Material Keywords
            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
            #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
            #pragma shader_feature_local_fragment _ _SPECGLOSSMAP
            #define _SPECULAR_COLOR // always on
            #pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA

            // URP Keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK

            // Unity Keywords
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            #include "RevealingFunctions.hlsl"
            #include "ShaderCalculationHelper.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float4 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);
                float3 positionWS : TEXCOORD2;
                half4 normalWS : TEXCOORD3;
                half4 tangentWS : TEXCOORD4;
                half4 bitangentWS : TEXCOORD5;
#ifdef _ADDITIONAL_LIGHTS_VERTEX
                half4 fogFactorAndVertexLight	: TEXCOORD6; // x: fogFactor, yzw: vertex light
#else
                half  fogFactor					: TEXCOORD6;
#endif
#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                float4 shadowCoord 				: TEXCOORD7;
#endif
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

            TEXTURE2D(_SpecGlossMap); 	
            SAMPLER(sampler_SpecGlossMap);

            // specular smoothness
            half4 SampleSpecularSmoothness(float2 uv, half alpha, half4 specColor, TEXTURE2D_PARAM(specMap, sampler_specMap)) {
                half4 specularSmoothness = half4(0.0h, 0.0h, 0.0h, 1.0h);
#ifdef _SPECGLOSSMAP
                specularSmoothness = SAMPLE_TEXTURE2D(specMap, sampler_specMap, uv) * specColor;
#elif defined(_SPECULAR_COLOR)
                specularSmoothness = specColor;
#endif

#ifdef _GLOSSINESS_FROM_BASE_ALPHA
                specularSmoothness.a = exp2(10 * alpha + 1);
#else
                specularSmoothness.a = exp2(10 * specularSmoothness.a + 1);
#endif
                return specularSmoothness;
            }

            // InputData
            
            void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData) {
                inputData = (InputData)0; // avoids "not completely initalized" errors

                inputData.positionWS = input.positionWS;

                half3 viewDirWS = half3(input.normalWS.w, input.tangentWS.w, input.bitangentWS.w);
                inputData.normalWS = TransformTangentToWorld(normalTS, half3x3(input.tangentWS.xyz, input.bitangentWS.xyz, input.normalWS.xyz));

                inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
                viewDirWS = SafeNormalize(viewDirWS);

                inputData.viewDirectionWS = viewDirWS;

#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                inputData.shadowCoord = input.shadowCoord;
#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
                inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
#else
                inputData.shadowCoord = float4(0, 0, 0, 0);
#endif

                // Fog
#ifdef _ADDITIONAL_LIGHTS_VERTEX
                inputData.fogCoord = input.fogFactorAndVertexLight.x;
                inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
#else
                inputData.fogCoord = input.fogFactor;
                inputData.vertexLighting = half3(0, 0, 0);
#endif

                inputData.bakedGI = SAMPLE_GI(input.lightmapUV, input.vertexSH, inputData.normalWS);
                //inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
                //inputData.shadowMask = SAMPLE_SHADOWMASK(input.lightmapUV);
            }

            Varyings vert (Attributes input)
            {
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS.xyz);

                output.vertex = vertexInput.positionCS;
                output.uv = TRANSFORM_TEX(input.uv, _Albedo);
                output.positionWS = vertexInput.positionWS;

                half3 viewDirWS = GetCameraPositionWS() - vertexInput.positionWS;
                half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);
                half fogFactor = ComputeFogFactor(vertexInput.positionCS.z);

                output.normalWS = half4(normalInput.normalWS, viewDirWS.x);
                output.tangentWS = half4(normalInput.tangentWS, viewDirWS.y);
                output.bitangentWS = half4(normalInput.bitangentWS, viewDirWS.z);

                OUTPUT_LIGHTMAP_UV(input.lightmapUV, unity_LightmapST, output.lightmapUV);
                OUTPUT_SH(output.normalWS.xyz, output.vertexSH);

#ifdef _ADDITIONAL_LIGHTS_VERTEX
                output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
#else
                output.fogFactor = fogFactor;
#endif

#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                output.shadowCoord = GetShadowCoord(vertexInput);
#endif
                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                
                half2 uv = input.uv;

                // uv scroll
                float2 scrolledUv = uv + float2(_Time.x * 0.1, 0);

                // sample the texture
                half4 texColor = SAMPLE_TEXTURE2D(_Albedo, sampler_linear_repeat, uv);
                half3 color = texColor.rgb *_Color.rgb;
                half alpha = texColor.z * _Color.a;

                // calculate distance from revealing center
                float distortion = SAMPLE_TEXTURE2D(_DistortionTex, sampler_linear_repeat, scrolledUv).r;
                float distortedDistanceFromCenterPoint = fitRange(distortion, 0, 1, -0.2, 0.2) + distance(input.positionWS, _RevealArea.xyz);
                alpha *= GetFadingBorder(distortedDistanceFromCenterPoint, _RevealArea, _Feather);

                if (alpha < 0.1) 
                {
                    //discard;
                }

                // apply noise pattern with doubled texture sampling
                float doubledNoise = SampleTextureWidhDoubledUv(_NoiseTilingOffset1, _NoiseTilingOffset2, uv, _NoiseTex, sampler_linear_repeat).r;

                // calculate feather for emission
                float featherAroundFadingBorder = GetFeatherAroundFadingBorder(distortedDistanceFromCenterPoint, _RevealArea, _Feather);
                float emissionLerpFactor = saturate(saturate(pow(doubledNoise.r * 2, 4)) + featherAroundFadingBorder);

                half3 normalTS = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap));
                half3 emission = _EmissionColor.rgb * emissionLerpFactor;
                half4 specular = SampleSpecularSmoothness(uv, alpha, half4(1,1,1,1), TEXTURE2D_ARGS(_SpecGlossMap, sampler_SpecGlossMap));
                half smoothness = specular.a;

                // Setup InputData
                InputData inputData;
                InitializeInputData(input, normalTS, inputData);

                // simple lighting
                half4 lighting = UniversalFragmentBlinnPhong(inputData, color, specular, smoothness, emission, alpha);


                return lighting;
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/InternalErrorShader"
}
