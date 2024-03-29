﻿Shader "StealthGame/RevealingDistortion_Lit"
{
    Properties
    {
        // Specular vs Metallic workflow
        [HideInInspector] _WorkflowMode("WorkflowMode", Float) = 1.0

        [MainColor] _BaseColor("BaseColor", Color) = (1,1,1,1)
        [MainTexture] _BaseMap("Albedo", 2D) = "white" {}

        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.1

        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
        _GlossMapScale("Smoothness Scale", Range(0.0, 1.0)) = 1.0
        _SmoothnessTextureChannel("Smoothness texture channel", Float) = 0

        [Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        _MetallicGlossMap("Metallic", 2D) = "white" {}

        _SpecColor("Specular", Color) = (0.2, 0.2, 0.2)
        _SpecGlossMap("Specular", 2D) = "white" {}

        [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
        [ToggleOff] _EnvironmentReflections("Environment Reflections", Float) = 1.0

        _BumpScale("Scale", Float) = 1.0
        _BumpMap("Normal Map", 2D) = "bump" {}

        _OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
        _OcclusionMap("Occlusion", 2D) = "white" {}

        [HDR]_EmissionColor("EmissionColor", Color) = (0,0,0)
        _EmissionMap("Emission", 2D) = "white" {}

        // Blending state
        [HideInInspector] _Surface("__surface", Float) = 0.0
        [HideInInspector] _Blend("__blend", Float) = 0.0
        [HideInInspector] _AlphaClip("__clip", Float) = 0.0
        //[HideInInspector] _SrcBlend("__src", Float) = 1.0
        //[HideInInspector] _DstBlend("__dst", Float) = 0.0
        //[HideInInspector] _ZWrite("__zw", Float) = 1.0
        //[HideInInspector] _Cull("__cull", Float) = 2.0

        _ReceiveShadows("Receive Shadows", Float) = 1.0
        // Editmode props
        [HideInInspector] _QueueOffset("Queue offset", Float) = 0.0

        [Header(RevealingParameters)]
        _Feather("Feather", Range(0,1)) = 0.1
        _NoiseTex("NoiseTex", 2D) = "white"{}
        _DistortionTex("DistortionTex", 2D) = "grey"{}
        _NoiseTilingOffset1("NoiseTilingOffset1", Vector) = (1,1,0,0)
        _NoiseTilingOffset2("NoiseTilingOffset2", Vector) = (1,1,0,0)
    }

    SubShader
    {
        // Universal Pipeline tag is required. If Universal render pipeline is not set in the graphics settings
        // this Subshader will fail. One can add a subshader below or fallback to Standard built-in to make this
        // material work with both Universal Render Pipeline and Builtin Unity Pipeline
        Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}
        LOD 300

        // ------------------------------------------------------------------
        //  Forward pass. Shades all light in a single pass. GI + emission + Fog
        Pass
        {
            // Lightmode matches the ShaderPassName set in UniversalRenderPipeline.cs. SRPDefaultUnlit and passes with
            // no LightMode tag are also rendered by Universal Render Pipeline
            Name "ForwardLit"
            Tags{"LightMode" = "UniversalForward"}

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            Cull Back

            HLSLPROGRAM
        // Required to compile gles 2.0 with standard SRP library
        // All shaders must be compiled with HLSLcc and currently only gles is not using HLSLcc by default
        #pragma prefer_hlslcc gles
        //#pragma exclude_renderers d3d11_9x
        #pragma target 5.0

        // -------------------------------------
        // Material Keywords
        #pragma shader_feature _NORMALMAP
        #pragma shader_feature _ALPHATEST_ON
        #pragma shader_feature _ALPHAPREMULTIPLY_ON
        #pragma shader_feature _EMISSION
        #pragma shader_feature _METALLICSPECGLOSSMAP
        #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
        #pragma shader_feature _OCCLUSIONMAP

        #pragma shader_feature _SPECULARHIGHLIGHTS_OFF
        #pragma shader_feature _ENVIRONMENTREFLECTIONS_OFF
        #pragma shader_feature _SPECULAR_SETUP
        #pragma shader_feature _RECEIVE_SHADOWS_OFF
        
        // -------------------------------------
        // Universal Pipeline keywords
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
        #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
        #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
        #pragma multi_compile _ _SHADOWS_SOFT
        #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

        // -------------------------------------
        // Unity defined keywords
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile_fog

        //--------------------------------------
        // GPU Instancing
        #pragma multi_compile_instancing

        #pragma vertex LitPassVertex
        #pragma fragment LitPassFragment

        #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"

        #ifndef UNIVERSAL_FORWARD_LIT_PASS_INCLUDED
        #define UNIVERSAL_FORWARD_LIT_PASS_INCLUDED

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "RevealingFunctions.hlsl"

        struct Attributes
        {
            float4 positionOS   : POSITION;
            float3 normalOS     : NORMAL;
            float4 tangentOS    : TANGENT;
            float2 texcoord     : TEXCOORD0;
            float2 lightmapUV   : TEXCOORD1;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct Varyings
        {
            float2 uv                       : TEXCOORD0;
            DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);
            float3 positionWS               : TEXCOORD2;
        #ifdef _NORMALMAP
            float4 normalWS                 : TEXCOORD3;    // xyz: normal, w: viewDir.x
            float4 tangentWS                : TEXCOORD4;    // xyz: tangent, w: viewDir.y
            float4 bitangentWS              : TEXCOORD5;    // xyz: bitangent, w: viewDir.z
        #else
            float3 normalWS                 : TEXCOORD3;
            float3 viewDirWS                : TEXCOORD4;
        #endif

            half4 fogFactorAndVertexLight   : TEXCOORD6; // x: fogFactor, yzw: vertex light

        #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            float4 shadowCoord              : TEXCOORD7;
        #endif

            float4 positionCS               : SV_POSITION;
            UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO
        };

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
        StructuredBuffer< RevealArea> _revealAreaBuffer;

        void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData)
        {
            inputData = (InputData)0;
            inputData.positionWS = input.positionWS;
        #ifdef _NORMALMAP
            half3 viewDirWS = half3(input.normalWS.w, input.tangentWS.w, input.bitangentWS.w);
            inputData.normalWS = TransformTangentToWorld(normalTS,
                half3x3(input.tangentWS.xyz, input.bitangentWS.xyz, input.normalWS.xyz));
        #else
            half3 viewDirWS = input.viewDirWS;
            inputData.normalWS = input.normalWS;
        #endif

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

            inputData.fogCoord = input.fogFactorAndVertexLight.x;
            inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
            inputData.bakedGI = SAMPLE_GI(input.lightmapUV, input.vertexSH, inputData.normalWS);
        }

        ///////////////////////////////////////////////////////////////////////////////
        //                  Vertex and Fragment functions                            //
        ///////////////////////////////////////////////////////////////////////////////

        // Used in Standard (Physically Based) shader
        Varyings LitPassVertex(Attributes input)
        {
            Varyings output = (Varyings)0;

            UNITY_SETUP_INSTANCE_ID(input);
            UNITY_TRANSFER_INSTANCE_ID(input, output);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

            VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
            VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
            half3 viewDirWS = GetCameraPositionWS() - vertexInput.positionWS;
            half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);
            half fogFactor = ComputeFogFactor(vertexInput.positionCS.z);

            output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);

        #ifdef _NORMALMAP
            output.normalWS = half4(normalInput.normalWS, viewDirWS.x);
            output.tangentWS = half4(normalInput.tangentWS, viewDirWS.y);
            output.bitangentWS = half4(normalInput.bitangentWS, viewDirWS.z);
        #else
            output.normalWS = NormalizeNormalPerVertex(normalInput.normalWS);
            output.viewDirWS = viewDirWS;
        #endif

            OUTPUT_LIGHTMAP_UV(input.lightmapUV, unity_LightmapST, output.lightmapUV);
            OUTPUT_SH(output.normalWS.xyz, output.vertexSH);

            output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
            output.positionWS = vertexInput.positionWS;
        #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.shadowCoord = GetShadowCoord(vertexInput);
        #endif

            output.positionCS = vertexInput.positionCS;

            return output;
        }

        // Used in Standard (Physically Based) shader
        half4 LitPassFragment(Varyings input) : SV_Target
        {
            UNITY_SETUP_INSTANCE_ID(input);
            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

            SurfaceData surfaceData;
            InitializeStandardLitSurfaceData(input.uv, surfaceData);

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
            if(length > 0)
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
            
            InputData inputData;
            InitializeInputData(input, surfaceData.normalTS, inputData);

            half4 color = UniversalFragmentPBR(inputData, surfaceData.albedo, surfaceData.metallic, surfaceData.specular, surfaceData.smoothness, surfaceData.occlusion, _EmissionColor * emissionLerpFactor, surfaceData.alpha * fadeAlpha);

            clip(fadeAlpha - _Cutoff);

            color.rgb = MixFog(color.rgb, inputData.fogCoord);
            return color;
        }

        #endif

        ENDHLSL
    }

    Pass
    {
        Name "ShadowCaster"
        Tags{"LightMode" = "ShadowCaster"}

        ZWrite On
        ZTest LEqual
        Cull[_Cull]

        HLSLPROGRAM
        // Required to compile gles 2.0 with standard srp library
        #pragma prefer_hlslcc gles
        #pragma exclude_renderers d3d11_9x
        #pragma target 2.0

        // -------------------------------------
        // Material Keywords
        #pragma shader_feature _ALPHATEST_ON

        //--------------------------------------
        // GPU Instancing
        #pragma multi_compile_instancing
        #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

        #pragma vertex ShadowPassVertex
        #pragma fragment ShadowPassFragment

        #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
        ENDHLSL
    }

    Pass
    {
        Name "DepthOnly"
        Tags{"LightMode" = "DepthOnly"}

        ZWrite On
        ColorMask 0
        Cull[_Cull]

        HLSLPROGRAM
        // Required to compile gles 2.0 with standard srp library
        #pragma prefer_hlslcc gles
        #pragma exclude_renderers d3d11_9x
        #pragma target 2.0

        #pragma vertex DepthOnlyVertex
        #pragma fragment DepthOnlyFragment

        // -------------------------------------
        // Material Keywords
        #pragma shader_feature _ALPHATEST_ON
        #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

        //--------------------------------------
        // GPU Instancing
        #pragma multi_compile_instancing

        #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
        ENDHLSL
    }


    }
                FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
