// functions for revealing distortion shaders

#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"

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

float4 SampleTextureWidhDoubledUv(float4 tilingOffset1, float4 tilingOffset2, float2 uv/*refer texutre to sample*/)
{
	return float4(1,1,0,0);
}