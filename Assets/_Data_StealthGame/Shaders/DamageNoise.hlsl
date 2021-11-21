
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
//#include "ShaderCalculationHelper.hlsl"
#include "MainObjectFunctions.hlsl"

// shader function for "custom function" node in ShaderGraph
// damage expressions
void DamageNoise_float(half3 baseColor, half3 secondaryColor, half3 highlightColor, float3 worldSpacePos, float baseMask, float4 glitchTexPattern, out half3 outputColor, out half mask)
{
    // copy of the original implementation
    float noise1 = ValueNoise(float3(floor(worldSpacePos.x * 0.01) * 1000, worldSpacePos.y * 40, floor(worldSpacePos.z * 0.01) * 1000) + _Time.zxy);
    float noise2 = ValueNoise(float3(floor(worldSpacePos.x * 0.01) * 2000, worldSpacePos.y * 60, floor(worldSpacePos.z * 0.01) * 2000) + _Time.yzx);
    float2 noiseGB = float2(pow(noise1, 5), noise2 * (1 - noise1));

    // clip completely transparent area
    clip(baseMask);

    outputColor = half4(baseColor, (half) baseMask);
    mask = baseMask;
    outputColor = lerp(outputColor, secondaryColor, noiseGB.x);

    // apply scanlines
    float horizontalScanlines = pow(sin((worldSpacePos.y + _Time.x) * 600) * 0.5 + 1, max(2, 10 * (1 - baseMask)));

    mask *= horizontalScanlines;
    clip(mask - 0.2);

    // apply glitch
    mask *= saturate(SamplePhase(glitchTexPattern.r, _Time.y + 0.3, 0.1) + (1 - SamplePhase(glitchTexPattern.a, _Time.y / 2 + 0.6, 0.1)));
    clip(mask);
    outputColor *= float3(1 + SamplePhase(glitchTexPattern.g, _Time.y, 0.1) * 4, 1, 1 + SamplePhase(glitchTexPattern.b, _Time.y, 0.1) * 6);

}
