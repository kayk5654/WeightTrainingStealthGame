
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"

// macros for each platforms
/*
#if defined(SHADER_API_D3D11)
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/API/D3D11.hlsl"
#elif defined(SHADER_API_METAL)
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/API/Metal.hlsl"
#elif defined(SHADER_API_VULKAN)
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/API/Vulkan.hlsl"
#elif defined(SHADER_API_SWITCH)
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/API/Switch.hlsl"
#elif defined(SHADER_API_GLCORE)
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/API/GLCore.hlsl"
#elif defined(SHADER_API_GLES3)
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/API/GLES3.hlsl"
#elif defined(SHADER_API_GLES)
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/API/GLES2.hlsl"
#else
#error unsupported shader api
#endif
*/
// functions for revealing distortion shaders

// get fading border for revealing effect
float GetFadingBorder(float distanceFromCenterPoint, float4 revealArea, float feather)
{
	return smoothstep(revealArea.w, revealArea.w - feather, distanceFromCenterPoint);
}

// get feather used for emission around fading border
float GetFeatherAroundFadingBorder(float distanceFromCenterPoint, float4 revealArea, float feather)
{
	float featherForCalculation = feather * 2;
	float feather1 = smoothstep(revealArea.w + featherForCalculation, revealArea.w, distanceFromCenterPoint);
	float feather2 = smoothstep(revealArea.w - featherForCalculation, revealArea.w, distanceFromCenterPoint);
	return feather1 * feather2;
}

// sample a texture with uv patterns with different tiling and offset parameters
float4 SampleTextureWidhDoubledUv(float4 tilingOffset1, float4 tilingOffset2, float2 uv, Texture2D texture2d, SamplerState sampler_texture2d)
{
	half4 sampledTexture1 = SAMPLE_TEXTURE2D(texture2d, sampler_texture2d, uv * tilingOffset1.xy + tilingOffset1.zw);
	half4 sampledTexture2 = SAMPLE_TEXTURE2D(texture2d, sampler_texture2d, uv * tilingOffset2.xy + tilingOffset2.zw);

	return sampledTexture1 * sampledTexture2;
}