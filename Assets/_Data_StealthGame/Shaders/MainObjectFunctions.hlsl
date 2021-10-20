
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"

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
