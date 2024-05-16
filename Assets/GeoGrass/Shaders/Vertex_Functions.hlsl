struct Attributes
{
    float3 positionOS : POSITION;
    float3 normalOS : NORMAL;
    float4 tangentOS : TANGENT;
    float4 uv1 : TEXCOORD1;
#if UNITY_ANY_INSTANCING_ENABLED
    uint instanceID : INSTANCEID_SEMANTIC;
#endif
};
struct Varyings
{
    float4 positionCS : SV_POSITION;
    float3 positionWS;
    float3 normalWS;
    float4 tangentWS;
    float3 viewDirectionWS;
};
struct PackedVaryings
{
    float4 positionCS : SV_POSITION;
    float3 interp0 : TEXCOORD0;
    float3 interp1 : TEXCOORD1;
    float4 interp2 : TEXCOORD2;
    float3 interp3 : TEXCOORD3;
};

PackedVaryings PackVaryings(Varyings input)
{
    PackedVaryings output;
    output.positionCS = input.positionCS;
    output.interp0.xyz = input.positionWS;
    output.interp1.xyz = input.normalWS;
    output.interp2.xyzw = input.tangentWS;
    output.interp3.xyz = input.viewDirectionWS;
    return output;
}

Varyings BuildVaryings(Attributes input)
{
    Varyings output = (Varyings)0;
    float3 positionWS = TransformObjectToWorld(input.positionOS);

    float3 normalWS = TransformObjectToWorldNormal(input.normalOS);

    output.positionWS = positionWS;
    output.normalWS = normalWS;
    return output;
}

PackedVaryings vert(Attributes input)
{
    Varyings output = (Varyings)0;
    output = BuildVaryings(input);
    PackedVaryings packedOutput = (PackedVaryings)0;
    packedOutput = PackVaryings(output);
    return packedOutput;
}