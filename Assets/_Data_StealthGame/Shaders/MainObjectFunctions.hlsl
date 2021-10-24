
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
#include "ShaderCalculationHelper.hlsl"
/* functions for sphere node shaders */

struct SurfaceInputs
{
	float3 WorldSpaceNormal;
	float3 WorldSpaceTangent;
	float3 WorldSpaceBiTangent;
};

// calculate normal for the single ring pattern
void GetRingNormal(float ringWidth, float objectSpacePosition, float3 objectSpaceNormal, SurfaceInputs surfaceInputs, int axis, out float3 ringNormal, out float ringMask)
{
	
	float3 normal;

	// calculate normal mask beside of the ring mask
	float normalWidth = 0.03;
	float shrunkRingWidth1 = ringWidth - normalWidth;
	float shrunkRingWidth2 = ringWidth - normalWidth * 0.5;
	float absPosAlongAxis = abs(objectSpacePosition);

	float normalGradient1 = smoothstep(shrunkRingWidth1, shrunkRingWidth2, absPosAlongAxis);
	float normalGradient2 = smoothstep(ringWidth, shrunkRingWidth2, absPosAlongAxis);

	float baseNormal = normalGradient1 * normalGradient2;

	// calculate base normal beside of the ring mask
	float objPosSign = objectSpacePosition / abs(objectSpacePosition);
	float3 baseNormalDirection = objectSpaceNormal + objPosSign;

	// split output by axis
	if (axis == 0)
	{
		// output x axis normal
		baseNormalDirection.y = 0;
		baseNormalDirection.z = 0;
	}
	else if(axis == 1)
	{
		// output y axis normal
		baseNormalDirection.x = 0;
		baseNormalDirection.z = 0;
	}
	else 
	{
		// output z axis normal
		baseNormalDirection.x = 0;
		baseNormalDirection.y = 0;
	}

	// convert normals from object space to tangent space

	// calculate ring mask
	float mask = step(absPosAlongAxis, shrunkRingWidth1);
	
}

// get ring pattern based on the object space position and normals for the pattern
void GetRingNormal3D(float3 ringWidth, float3 objectSpacePosition, out float3 ringNormal, out float ringMask)
{
	
}

// get tint color applied for farther mesh
half3 GetFarTintColor(half3 originalColor, half3 tintColor, float3 worldSpacePosition)
{
	float distFromCamera = distance(_WorldSpaceCameraPos, worldSpacePosition);
	distFromCamera = saturate(fitRange(distFromCamera, 1.5, 3.5, 0, 1));

	half3 farTintColor = normalize(tintColor) * ((originalColor.x + originalColor.y + originalColor.z) / 3);

	return lerp(originalColor, farTintColor, distFromCamera);
}

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

// hue shift
half3 HueShift(half3 color, float shift)
{
	float3 compareVector = float3(0.55735, 0.55735, 0.55735);
	float3 p = compareVector * dot(compareVector, color);
	float3 u = color - p;
	float3 v = cross(compareVector, u);

	float multiplier = 6.2832;
	color = u * cos(shift * multiplier) + v * sin(shift * multiplier) + p;
	return color;
}

// define areas not to render pixels; input is assumed to be a value sampled from a grayscale texture
float DropPixel(float phase, float time, float range)
{
	float undropPhase = saturate(step((time - range)%1, phase) + step(phase, (time + range)%1));
	undropPhase = 1 - undropPhase;
	return undropPhase;
}
