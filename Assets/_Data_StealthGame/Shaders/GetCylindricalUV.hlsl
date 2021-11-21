
float fitRange(float value, float inMin, float inMax, float outMin, float outMax)
{
	float lerpFactor = lerp(0, 1, (clamp(value, inMin, inMax) - inMin) / (inMax - inMin));
	return lerp(outMin, outMax, lerpFactor);
}

// shader function for "custom function" node in ShaderGraph
// calculate cylindrical uv
void GetCylindricalUV_float(float3 worldPosition, out float2 uv)
{
	float2 cylindricalAngle = (worldPosition - mul(unity_ObjectToWorld, float3(0, 0, 0)).xyz).xz;
	cylindricalAngle = normalize(cylindricalAngle);
	uv = float2(fitRange(atan2(cylindricalAngle.x, cylindricalAngle.y), -3.14, 3.14, 0, 1), worldPosition.y) + 0.5;
}

