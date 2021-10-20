// fit a value in a specific range to another specific range
float fitRange(float value, float inMin, float inMax, float outMin, float outMax)
{
	float lerpFactor = lerp(0, 1, (clamp(value, inMin, inMax) - inMin) / (inMax - inMin));
	return lerp(outMin, outMax, lerpFactor);
}

// convert degree to radian angle
float DegreesToRadians(float degree) 
{
	return degree * 3.14 / 180;
}

float3 DegreesToRadians(float3 degrees)
{
	float3 radians = float3(DegreesToRadians(degrees.x), DegreesToRadians(degrees.y), DegreesToRadians(degrees.z));
	return radians;
}

// get rotation matrix to rotate spacified angle along x axis
float4x4 GetRotationMatrixAlongXAxis(float radians)
{
	float4x4 rotMatrix = 
			(1, 0, 0, 1,
			0, cos(radians), -sin(radians), 0,
			0, -sin(radians), cos(radians), 0,
			0, 0, 0, 1);
	return rotMatrix;
}

// get rotation matrix to rotate spacified angle along y axis
float4x4 GetRotationMatrixAlongYAxis(float radians)
{
	float4x4 rotMatrix =
			(cos(radians), 0, sin(radians), 0,
			0, 1, 0, 0,
			-sin(radians), 0, cos(radians), 0,
			0, 0, 0, 1);
	return rotMatrix;
}

// get rotation matrix to rotate spacified angle along y axis
float4x4 GetRotationMatrixAlongZAxis(float radians)
{
	float4x4 rotMatrix =
			(cos(radians), -sin(radians), 0, 0,
			sin(radians), cos(radians), 0, 0,
			0, 0, 1, 0,
			0, 0, 0, 1);
	return rotMatrix;
}