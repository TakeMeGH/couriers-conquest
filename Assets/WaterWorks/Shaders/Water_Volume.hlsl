#ifndef Water_Volume
#define Water_Volume

CBUFFER_START(UnityPerMaterial)
Texture2D _MainTex;
sampler sampler_MainTex;
float4 Albedo;
float density;
float3 pos;
float3 bounds;
CBUFFER_END

float2 random2D(float2 UV, float offset)
{
    float2x2 m = float2x2(15.27f, 47.63f, 99.41f, 89.98f);
    UV = frac(sin(mul(UV, m)));
    return float2(sin(UV.y * +offset) * 0.5f + 0.5f, cos(UV.x * offset) * 0.5f + 0.5f);
}

float random3D(float3 uv)
{
    float Coord = (uv.x + uv.y + uv.z);
    float2 _uv = float2(Coord, Coord);
    float2 noise = (frac(sin(dot(_uv, float2(12.9898, 78.233) * 2.0)) * 43758.5453));
    return (abs(noise.x + noise.y) - 1);
}

float2 Cellular(float2 UV, float AngleOffset)
{
    float2 g = floor(UV);
    float2 f = frac(UV);
    float2 res = float2(8, 8);

    [unroll]
    for (int y = -1; y <= 1; y++)
    {
        [unroll]
        for (int x = -1; x <= 1; x++)
        {
            float2 lattice = float2(x, y);
            float2 offset = random2D(lattice + g, AngleOffset);
            float dist = distance(lattice + offset, f);

            if (dist < res.x)
            {
                res.y = length(offset);
                res.x = dist;
            }
        }
    }

    return res;
}

float2 rayBoxDst(float3 boundsMin, float3 boundsMax, float3 rayOrigin, float3 invRaydir)
{
    float3 t0 = (boundsMin - rayOrigin) * invRaydir;
    float3 t1 = (boundsMax - rayOrigin) * invRaydir;
    float3 tmin = min(t0, t1);
    float3 tmax = max(t0, t1);

    float dstA = max(max(tmin.x, tmin.y), tmin.z);
    float dstB = min(tmax.x, min(tmax.y, tmax.z));

    float dstToBox = max(0, dstA);
    float dstInsideBox = max(0, dstB - dstToBox);
    return float2(dstToBox, dstInsideBox);
}

float3 GetRay(float2 screenPos)
{
    float3 viewVector = mul(unity_CameraInvProjection, float4(screenPos * 2 - 1, 0, -1));
    float3 viewDir = mul(unity_CameraToWorld, float4(viewVector, 0));
    float viewLength = length(viewDir);
    float3 ray = viewDir / viewLength;

    return ray;
}

float SceneDepth(float2 UV)
{
    return LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV), _ZBufferParams);
}

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;

    pos.xz += _WorldSpaceCameraPos.xz;

    float3 posBL = pos - bounds / 2;
    float3 posTR = pos + bounds / 2;

    float3 viewVector = mul(unity_CameraInvProjection, float4(IN.ScreenPosition.xy * 2 - 1, 0, -1));
    float3 viewDir = mul(unity_CameraToWorld, float4(viewVector, 0));
    float viewLength = length(viewDir);

    float3 ray = GetRay(IN.ScreenPosition.xy);

    float2 boxDist = rayBoxDst(posBL, posTR, _WorldSpaceCameraPos, 1 / ray);

    float distToBox = boxDist.x;
    float distInBox = boxDist.y;

    float3 entryPoint = _WorldSpaceCameraPos + ray * distToBox;

    float3 rayPos = entryPoint + ray * viewLength;

    float depth = SceneDepth(IN.ScreenPosition.xy) * viewLength;

    if (distInBox == 0 || distToBox > depth)
    {
        surface.BaseColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.ScreenPosition.xy / IN.ScreenPosition.w);
        return surface;
    }

    float cumulativeDensity = 0;
    float cumulativeDensityNoNoise = 0;
    float lighting = 0;
    float distTravelled = 0;
    float stepSize = 0.5;
    int i = 0;
    UNITY_LOOP
        for (i = 0; i < 250 && distTravelled + distToBox < depth && distTravelled < distInBox; i++)
        {
            float shadowAtten = 1;
            float shadowAttenL = 1;
            float shadowAttenR = 1;
            float pointDensity;
            float distMultiplier = 1;

            float yPos = rayPos.y - pos.y;
            yPos += bounds.y / 2;
            yPos /= bounds.y;
            float surfaceDist = pow(yPos, 200);
            //surfaceDist = 0;
            yPos = 1 - yPos;
            yPos = pow(yPos, 50);

            float noiseValue = Cellular(rayPos.xz * 0.2, (_Time * 50));
            pointDensity = clamp((density * (1 - noiseValue)) + (yPos + surfaceDist), 0, 1);
            cumulativeDensity += pointDensity * stepSize;
            cumulativeDensityNoNoise += 0.2 * stepSize;
            lighting += 0.05 * (1 - noiseValue);

            if (1 - exp(-cumulativeDensity) > 1)
            {
                break;
            }

            float jitter = 0.9;
            float randOffset = (random3D(rayPos) * (2 * (1 - jitter))) + (jitter);

            distTravelled += stepSize * randOffset;

            rayPos += ray * stepSize * randOffset;

            if (distTravelled > 70)
            {
                stepSize += 10 * distMultiplier;
                distMultiplier += 5;
            }
        }

    float totalDensity = exp(-cumulativeDensity);
    float totalDensityNoNoise = 1 - exp(-cumulativeDensityNoNoise);
    float totalLighting = pow(exp(-lighting), 0.5);

    float2 refraction = sin(totalDensity) * 0.1;
    float3 background = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, (IN.ScreenPosition.xy / IN.ScreenPosition.w) + refraction);

    surface.BaseColor = lerp(background * ((totalLighting * 1) + 0.1), Albedo * (totalLighting + 0.8), (((1 - totalDensity) * (1 - density)) + density) * totalDensityNoNoise);
    return surface;
}
#endif