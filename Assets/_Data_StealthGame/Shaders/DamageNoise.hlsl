
// return random value, 0 to 1
float3 Random(float3 position)
{
	return frac(sin(cross(position, float3(12.9898, 78.233, 41.028913))));
}

// return value noise
float3 ValueNoise(float3 position)
{
	// get int and fraction of given position
	float3 integer = floor(position);
	float3 fraction = frac(position);

	// sample 8 points
	float3 n000 = Random(integer);
	float3 n001 = Random(integer + float3(0, 0, 1));
	float3 n101 = Random(integer + float3(1, 0, 1));
	float3 n100 = Random(integer + float3(1, 0, 0));
	float3 n010 = Random(integer + float3(0, 1, 0));
	float3 n011 = Random(integer + float3(0, 1, 1));
	float3 n110 = Random(integer + float3(1, 1, 0));
	float3 n111 = Random(integer + float3(1, 1, 1));

	// smooth interpolation
	// cubic hermine curve
	float3 u = fraction * fraction * (3.0 - 2.0 * fraction);

	return lerp(lerp(lerp(n000, n100, u.x), lerp(n010, n110, u.x), u.y), lerp(lerp(n001, n101, u.x), lerp(n011, n111, u.x), u.y), u.z);
}

// define areas not to render pixels; input is assumed to be a value sampled from a grayscale texture
float SamplePhase(float phase, float samplePoint, float range)
{
	float sampledPhase = abs((samplePoint % 1) - phase) < range ? 1 : 0;

	return sampledPhase;
}

// shader function for "custom function" node in ShaderGraph
// damage expressions
void DamageNoise_float(half3 baseColor, half3 secondaryColor, half3 highlightColor, float3 worldSpacePos, float baseMask, float4 glitchTexPattern, out half3 outputColor, out half mask)
{
	// copy of the original implementation
    float noise1 = ValueNoise(float3(floor(worldSpacePos.x * 0.01) * 1000, worldSpacePos.y * 40, floor(worldSpacePos.z * 0.01) * 1000) + _Time.zxy);
    float noise2 = ValueNoise(float3(floor(worldSpacePos.x * 0.01) * 2000, worldSpacePos.y * 60, floor(worldSpacePos.z * 0.01) * 2000) + _Time.yzx);
    float2 noiseGB = float2(pow(noise1, 5), noise2 * (1 - noise1));

    outputColor = half4(baseColor, (half) baseMask);
    mask = baseMask;
    outputColor = lerp(outputColor, secondaryColor, noiseGB.x);

    // apply scanlines
    float horizontalScanlines = pow(sin((worldSpacePos.y + _Time.x) * 600) * 0.5 + 1, max(2, 10 * (1 - baseMask)));

    mask *= horizontalScanlines;
    //clip(mask - 0.2);

    // apply glitch
    mask *= saturate(SamplePhase(glitchTexPattern.r, _Time.y + 0.3, 0.1) + (1 - SamplePhase(glitchTexPattern.a, _Time.y / 2 + 0.6, 0.1)));
    //clip(mask);
    outputColor *= float3(1 + SamplePhase(glitchTexPattern.g, _Time.y, 0.1) * 4, 1, 1 + SamplePhase(glitchTexPattern.b, _Time.y, 0.1) * 6);

	//outputColor = baseMask < 0.5 ? baseColor : outputColor;

}
