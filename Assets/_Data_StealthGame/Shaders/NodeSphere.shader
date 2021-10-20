Shader "StealthGame/NodeSphere"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [Space(10)]
        [Header(Rotated pattern)]
        _Speed ("Speed", Float) = 1
        [Space(10)]
        [Header(Color)]
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        [HDR]_EmitAreaColor("Emit Area Color", Color) = (1, 1, 1, 1)
        _FarTintColor("Far Tint Color", Color) = (0.5, 0, 1, 1)
        [Space(10)]
        [Header(Textures)]
        _NormalTex("Normal Map", 2D) = "bump"{}
        _OcclusionTex("Occlusion Map", 2D) = "white"{}
        _IridescenceTex("Iridescence Map", 2D) = "black" {}
        _TilingOffset("Tiling / Offset", Vector) = (1, 1, 0, 0)
    }
    SubShader
    {
        Tags{"RenderType" = "Opaque" "Queue"="Transparent" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}
        Blend SrcAlpha OneMinusSrcAlpha
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
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl" // VertexPositionInput, etc.
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl" 

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 positionWS : TEXCOORD1;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_NormalTex);
            TEXTURE2D(_OcclusionTex);
            TEXTURE2D(_IridescenceTex);

            // cbuffer contains exposed properties
            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            float _Speed;
            half4 _EmitAreaColor;
            half4 _FarTintColor;
            float4 _TilingOffset;
            CBUFFER_END

            Varyings vert (Attributes input)
            {
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

                output.vertex = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                
                // sample the texture
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                return col;
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/InternalErrorShader"
}
