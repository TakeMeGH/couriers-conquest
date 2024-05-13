#ifndef SSRef
#define SSRef

#define _GBUFFER_NORMALS_OCT
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"

float SceneDepth(float2 UV)
{
	return LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV), _ZBufferParams);
}

float3 SceneColor(float2 UV)
{
	return SHADERGRAPH_SAMPLE_SCENE_COLOR(UV);
}

float2 WorldToScreenPoint(float3 pos)
{
	pos = normalize(pos - _WorldSpaceCameraPos) * (_ProjectionParams.y + (_ProjectionParams.z - _ProjectionParams.y)) + _WorldSpaceCameraPos;
	float3 toCam = mul(unity_WorldToCamera, pos);
	float camPosZ = toCam.z;
	float height = 2 * camPosZ / unity_CameraProjection._m11;
	float width = _ScreenParams.x / _ScreenParams.y * height;
	float2 uvCoords;
	uvCoords.x = (toCam.x + width / 2) / width;
	uvCoords.y = (toCam.y + height / 2) / height;
	return uvCoords;
}

float3 DecodeNormal(float3 enc)
{
	float kScale = 1.7777;
	float3 nn = enc.xyz * float3(2 * kScale, 2 * kScale, 0) + float3(-kScale, -kScale, 1);
	float g = 2.0 / dot(nn.xyz, nn.xyz);
	float3 n;
	n.xy = g * nn.xy;
	n.z = g - 1;
	return n;
}

float3 ReconstructPosFromDepth(float depth, float2 screenPos)
{
	float3 viewVector = mul(unity_CameraInvProjection, float4(screenPos.xy * 2 - 1, 0, -1));
	float3 viewDirection = mul(unity_CameraToWorld, float4(viewVector, 0));
	float3 cameraDirection = (-1 * mul((float3x3)UNITY_MATRIX_M, transpose(mul(UNITY_MATRIX_I_M, UNITY_MATRIX_I_V))[2].xyz));

	float ViewDotCam = dot(viewDirection, cameraDirection);
	float3 ViewDivCam = viewDirection / ViewDotCam;
	float3 ViewMulDepth = ViewDivCam * depth;
	float3 pos = ViewMulDepth + _WorldSpaceCameraPos;
	return pos;
}

float ReconstructDepthFromPos(float3 pos, float2 screenPos)
{
	float3 viewVector = mul(unity_CameraInvProjection, float4(screenPos.xy * 2 - 1, 0, -1));
	float3 viewDirection = mul(unity_CameraToWorld, float4(viewVector, 0));
	float3 cameraDirection = (-1 * mul((float3x3)UNITY_MATRIX_M, transpose(mul(UNITY_MATRIX_I_M, UNITY_MATRIX_I_V))[2].xyz));

	float ViewDotCam = dot(viewDirection, cameraDirection);
	float3 ViewDivCam = viewDirection / ViewDotCam;

	pos -= _WorldSpaceCameraPos;
	float depth = (pos / ViewDivCam).x;

	return depth;
}

void SSR_float(float3 viewDir, float stepSize, float4 screenPos, float samples, float thickness, float smoothness, float3 _normal, float3 _position, bool reconstructDepth, out float3 col)
{
	viewDir.x = -viewDir.x;

	float3 reflectionPos = ReconstructPosFromDepth(SceneDepth(screenPos.xy), screenPos);
	float3 normal = SampleSceneNormals(screenPos.xy);
	normal = float3(0, 1, 0);
	normal = DecodeNormal(float3(0, 1, 0));
	normal = _normal;
	float3 reflection = reflect(normalize(viewDir), NormalizeNormalPerPixel(normal));

	reflectionPos += normalize(reflection) * distance(reflectionPos.y, _WorldSpaceCameraPos.y);
	float2 refScreenPos = WorldToScreenPoint(reflectionPos);

	float dist = distance(_WorldSpaceCameraPos, reflectionPos);

	float depth = SceneDepth(refScreenPos);

	float3 depthPos = ReconstructPosFromDepth(depth, screenPos);

	if (reconstructDepth)
	{
		depthPos = _position;
	}

	float depthDist = distance(_WorldSpaceCameraPos, depthPos);

	bool foundSSR = false;
	UNITY_LOOP
	for (int j = 0; j < samples; j++)
	{
		float deltaDepth = depthDist - dist;

		reflectionPos += normalize(reflection) * deltaDepth / 2;
		refScreenPos = WorldToScreenPoint(reflectionPos);

		dist = distance(_WorldSpaceCameraPos, reflectionPos);

		depth = SceneDepth(refScreenPos);
		depthPos = ReconstructPosFromDepth(depth, screenPos);
		depthDist = distance(_WorldSpaceCameraPos, depthPos);

		col = min(SceneColor(refScreenPos), 5);

		if (refScreenPos.y < 0.1)
		{
			float blend = (clamp(refScreenPos.y, 0, 1)) * 10;
			col = lerp(col, 0.5, 1 - blend);
		}

		if (refScreenPos.y > 0.9)
		{
			float blend = (1 - clamp(refScreenPos.y, 0, 1)) * 10;
			col = lerp(col, 0.5, 1 - blend);
		}

		if (refScreenPos.x < 0.1)
		{
			float blend = (clamp(refScreenPos.x, 0, 1)) * 10;
			col = lerp(col, 0.5, 1 - blend);
		}

		if (refScreenPos.x > 0.9)
		{
			float blend = (1 - clamp(refScreenPos.x, 0, 1)) * 10;
			col = lerp(col, 0.5, 1 - blend);
		}

		if (deltaDepth <= 100)
		{
			foundSSR = true;
		}

		if (deltaDepth <= thickness)
		{
			foundSSR = true;
			break;
		}
	}

	if (!foundSSR && 1 != 1)
	{
		reflectionPos = ReconstructPosFromDepth(SceneDepth(screenPos.xy), screenPos);
		reflectionPos += normalize(reflection) * 11;
		refScreenPos = WorldToScreenPoint(reflectionPos);

		col = min(SceneColor(refScreenPos), 5);

		if (refScreenPos.y < 0.1)
		{
			float blend = (clamp(refScreenPos.y, 0, 1)) * 10;
			col = lerp(col, 0.5, 1 - blend);
		}

		if (refScreenPos.y > 0.9)
		{
			float blend = (1 - clamp(refScreenPos.y, 0, 1)) * 10;
			col = lerp(col, 0.5, 1 - blend);
		}

		if (refScreenPos.x < 0.1)
		{
			float blend = (clamp(refScreenPos.x, 0, 1)) * 10;
			col = lerp(col, 0.5, 1 - blend);
		}

		if (refScreenPos.x > 0.9)
		{
			float blend = (1 - clamp(refScreenPos.x, 0, 1)) * 10;
			col = lerp(col, 0.5, 1 - blend);
		}
	}
}

#endif