
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
//#include "ShaderCalculationHelper.hlsl"
#include "MainObjectFunctions.hlsl"

// damage expressions
void DamageNoise(half3 baseColor, half3 secondaryColor, half3 highlightColor, float3 worldSpacePos, float baseMask, out half3 outputColor, out half mask)
{
    // project a texture cylindrically; the origin of the cylinder is the pivot of the mesh
    float2 cylindricalAngle = (worldSpacePos - mul(unity_ObjectToWorld, float3(0, 0, 0)).xyz).xz;
    cylindricalAngle = normalize(cylindricalAngle);
    float2 decalUv1 = float2(fitRange(atan2(cylindricalAngle.x, cylindricalAngle.y), -3.14, 3.14, 0, 1), worldSpacePos.y) + 0.5;

    // apply tiling and offset on uv
    decalUv1 = decalUv1 * _MainTex_ST.xy + _MainTex_ST.zw;

    // sample texture by object space opaque mesh position
    float4 texPattern = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, decalUv1);
    float dist = length(objectSpacePosBehind);
    float feather = 0.4;
    float affectArea = smoothstep(0, feather, length(input.positionVS) - abs(sceneDepth));
    
    // copy of the original implementation
    float noise1 = ValueNoise(float3(floor(worldSpacePos.x * 0.01) * 1000, worldSpacePos.y * 40, floor(worldSpacePos.z * 0.01) * 1000) + _Time.zxy);
    float noise2 = ValueNoise(float3(floor(worldSpacePos.x * 0.01) * 2000, worldSpacePos.y * 60, floor(worldSpacePos.z * 0.01) * 2000) + _Time.yzx);
    float2 noiseGB = float2(pow(noise1, 5), noise2 * (1 - noise1));

    // define fading area
    float revealDist = distance(_RevealArea.xyz, input.positionWSOriginal);
    float fadingArea = GetFadingBorder(revealDist, _RevealArea, _Feather);
    baseMask *= fadingArea;

    // clip completely transparent area
    clip(baseMask);

    half4 outputColor = half4(baseColor, (half) baseMask);
    half mask = baseMask;
    outputColor = lerp(outputColor, secondaryColor, noiseGB.x);

    // apply scanlines
    float horizontalScanlines = pow(sin((worldSpacePos.y + _Time.x) * 600) * 0.5 + 1, max(2, 10 * (1 - baseMask)));

    mask *= horizontalScanlines;
    clip(mask - 0.2);

    // apply glitch
    mask *= saturate(SamplePhase(texPattern.r, _Time.y + 0.3, 0.1) + (1 - SamplePhase(texPattern.a, _Time.y / 2 + 0.6, 0.1)));
    clip(mask);
    outputColor *= float3(1 + SamplePhase(texPattern.g, _Time.y, 0.1) * 4, 1, 1 + SamplePhase(texPattern.b, _Time.y, 0.1) * 6);

}
