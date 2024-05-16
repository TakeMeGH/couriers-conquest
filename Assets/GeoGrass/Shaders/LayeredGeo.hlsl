Texture2D GrassTex;
sampler sampler_GrassTex;
Texture2D DirtTex;
sampler sampler_DirtTex;
Texture2D RockTex;
sampler sampler_RockTex;

float4 Albedo;
float Tiling1;
float Tiling2;
float Tiling3;
float Tiling4;
float Height;
float Smoothness;
float NormalStrength = 0;
float TexBlend;
float Cutoff;
float AlphaClip;

#define GRASS_LAYERS 11

struct GeomData
{
    float4 positionCS : SV_POSITION;
    float3 positionWS : TEXCOORD0;
    float3 normalWS : TEXCOORD1;
    float4 tangentWS : TEXCOORD2;
    float3 viewDirectionWS : TEXCOORD3;
#if defined(LIGHTMAP_ON)
    float2 lightmapUV : TEXCOORD4;
#endif
#if !defined(LIGHTMAP_ON)
    float3 sh : TEXCOORD5;
#endif
    float4 fogFactorAndVertexLight : TEXCOORD6;
    float4 shadowCoord : TEXCOORD7;
    float height : TEXCOORD8;
};

[maxvertexcount(3 * GRASS_LAYERS)]
void Geometry(triangle GeomData input[3], inout TriangleStream<GeomData> triStream)
{
    int dist = distance(_WorldSpaceCameraPos, (input[0].positionWS + input[1].positionWS + input[2].positionWS) / 3);
    float maxDist = 330;
    dist = maxDist - min(dist, maxDist);
    dist /= (maxDist / GRASS_LAYERS);
    dist = max(dist, 1);

    for (float i = 0; i < GRASS_LAYERS; i++)
    {
        GeomData vert0 = input[0];
        GeomData vert1 = input[1];
        GeomData vert2 = input[2];

        float heightOffset = i / GRASS_LAYERS * Height;
        vert0.positionWS += vert0.normalWS * heightOffset;
        vert1.positionWS += vert1.normalWS * heightOffset;
        vert2.positionWS += vert2.normalWS * heightOffset;

        vert0.positionCS = TransformWorldToHClip(vert0.positionWS);
        vert1.positionCS = TransformWorldToHClip(vert1.positionWS);
        vert2.positionCS = TransformWorldToHClip(vert2.positionWS);

        vert0.height = i / GRASS_LAYERS;
        vert1.height = i / GRASS_LAYERS;
        vert2.height = i / GRASS_LAYERS;

        triStream.Append(vert0);
        triStream.Append(vert1);
        triStream.Append(vert2);
        triStream.RestartStrip();
    }
}

float3 GetViewDirectionFromPosition(float3 positionWS)
{
    return normalize(_WorldSpaceCameraPos - positionWS);
}

float3 WSUV(float _TilingWS, float3 worldPos)
{
    return worldPos * _TilingWS;
}

float2 Unity_GradientNoise_Dir_float(float2 p)
{
    p = p % 289;
    float x = float(34 * p.x + 1) * p.x % 289 + p.y;
    x = (34 * x + 1) * x % 289;
    x = frac(x / 41) * 2 - 1;
    return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
}

void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
{
    float2 p = UV * Scale;
    float2 ip = floor(p);
    float2 fp = frac(p);
    float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
    float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
    float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
    float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
    fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
    Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
}

inline float2 Unity_Voronoi_RandomVector_float(float2 UV, float offset)
{
    float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
    UV = frac(sin(mul(UV, m)));
    return float2(sin(UV.y * +offset) * 0.5 + 0.5, cos(UV.x * offset) * 0.5 + 0.5);
}

void Unity_Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
{
    float2 g = floor(UV * CellDensity);
    float2 f = frac(UV * CellDensity);
    float t = 8.0;
    float3 res = float3(8.0, 0.0, 0.0);

    for (int y = -1; y <= 1; y++)
    {
        for (int x = -1; x <= 1; x++)
        {
            float2 lattice = float2(x, y);
            float2 offset = Unity_Voronoi_RandomVector_float(lattice + g, AngleOffset);
            float d = distance(lattice + offset, f);

            if (d < res.x)
            {
                res = float3(d, offset.x, offset.y);
                Out = res.x;
                Cells = res.y;
            }
        }
    }
}

float3 GetSpecular(float _smoothness, float3 viewDir, float3 normalWS)
{
    _smoothness = 1 - _smoothness;

    Light mainLight = GetMainLight(0);
    float3 color = mainLight.color;

    float3 halfDir = SafeNormalize(_MainLightPosition + viewDir);
    float nh = saturate(dot(normalWS, halfDir));
    float lh = saturate(dot(_MainLightPosition, halfDir));
    float d = nh * nh * (_smoothness * _smoothness - 1.0) + 1.00001;
    float normalizationTerm = _smoothness * 4.0 + 2.0;
    float specularTerm = _smoothness * _smoothness;
    specularTerm /= (d * d) * max(0.1, lh * lh) * normalizationTerm;
    return specularTerm * (1 - _smoothness);
}

float3 GetNormalFromHeight(float T, float B, float L, float R, float Strength)
{
    float RsL = R - L;
    float TsB = T - B;
    float RLdS = RsL / 0.001;
    float TBdS = TsB / 0.001;
    float3 normal = float3(RLdS, TBdS, 1);
    normal = normalize(normal);
    //normal = float3(normal.xy * Strength, lerp(1, normal.z, saturate(Strength)));
    normal.z *= Strength;
    return normal;
}

float3 NormalBlend(float3 A, float3 B)
{
    return normalize(float3(A.rg + B.rg, A.b * B.b));
}

float4 Fragment(GeomData input) : SV_Target
{
    float3 UV = WSUV(1, input.positionWS);

    float4 grass = SAMPLE_TEXTURE2D(GrassTex, sampler_GrassTex, UV.xz * Tiling1);
    float4 dirt = SAMPLE_TEXTURE2D(DirtTex, sampler_DirtTex, UV.xz * Tiling2);
    float4 rockXZ = SAMPLE_TEXTURE2D(RockTex, sampler_RockTex, UV.xz * Tiling4);
    float4 rockXY = SAMPLE_TEXTURE2D(RockTex, sampler_RockTex, UV.xy * Tiling4);
    float4 rockYZ = SAMPLE_TEXTURE2D(RockTex, sampler_RockTex, float2(UV.z, UV.y) * Tiling4);

    float Perlin;
    Unity_GradientNoise_float(UV.xz, Tiling3, Perlin);

    float4 col = lerp(dirt, grass, smoothstep(TexBlend, TexBlend + 0.4, Perlin));

    half3 blend = abs(input.normalWS);
    blend /= dot(blend, 1);
    float4 rock = rockYZ * blend.x + rockXZ * blend.y + rockXY * blend.z;

    float normalDot = max(dot(input.normalWS, float3(0, 1, 0)), 0);
    normalDot = smoothstep(Cutoff, Cutoff + 0.1, normalDot);

    col = lerp(rock, col, normalDot);

    float3 A = col;
    float Alpha = (A.x + A.y + A.z) / 3;

    //input.normalWS = NormalBlend(input.normalWS, float4((2 * GetNormalFromHeight(ddy(Alpha), Alpha, Alpha, ddx(Alpha), NormalStrength)) + 1, 1));

    col *= Albedo;

    float3 specular = GetSpecular(Smoothness, GetViewDirectionFromPosition(input.positionWS), input.normalWS);
    col += float4(specular / 2, 0);

    Perlin *= 0.6;
    Perlin += 0.4;

    float nDotl = dot(NormalizeNormalPerPixel(input.normalWS), _MainLightPosition.xyz);
    nDotl = (max(nDotl, 0) * 0.75) + 0.25;

#if SHADOWS_SCREEN
    half4 clipPos = TransformWorldToHClip(input.positionWS);
    half4 shadowCoord = ComputeScreenPos(clipPos);
#else
    half4 shadowCoord = TransformWorldToShadowCoord(input.positionWS);
#endif
    Light mainLight = GetMainLight(shadowCoord);
    float3 dir = mainLight.direction;
    float3 color = mainLight.color;
    float distAtten = mainLight.distanceAttenuation;
    float shadowAtten = mainLight.shadowAttenuation;

    clip(clamp(Alpha * Perlin, 0, 1) - (AlphaClip * input.height));
    //return float4(input.normalWS, 1);
    return float4(col.xyz * (color / 3) * nDotl * distAtten * shadowAtten, Alpha);
}