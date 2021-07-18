// fit a value in a specific range to another specific range
float fitRange(float value, float inMin, float inMax, float outMin, float outMax)
{
	float lerpFactor = lerp(0, 1, (clamp(value, inMin, inMax) - inMin) / (inMax - inMin));
	return lerp(outMin, outMax, lerpFactor);
}