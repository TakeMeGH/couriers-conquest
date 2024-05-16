Shader "GrassLayersGeo"
{
    Properties
    {
        GrassTex("Grass Texture", 2D) = "white" {}
        DirtTex("Dirt Texture", 2D) = "white" {}
        RockTex("Rock Texture", 2D) = "white" {}
        Albedo("Albedo", Color) = (0.08564636, 0.2641509, 0, 1)
        Tiling1("Texture 1 Tiling", Float) = 1
        Tiling2("Texture 2 Tiling", Float) = 1
        Tiling4("Rock Tiling", Float) = 1
        Tiling3("Blend Tiling", Float) = 1
        Height("Height", Float) = 0.5
        Smoothness("Smoothness", Range(0, 1)) = 0.5
        //NormalStrength("Normal Strength", Float) = 0.5
        TexBlend("TexBlend", Range(0, 1)) = 0
        Cutoff("Cutoff", Range(-1, 1)) = 0.5
        AlphaClip("AlphaClip", Range(0, 1)) = 0.5
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
            "UniversalMaterialType" = "Lit"
            "Queue" = "Geometry"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }

    Cull Back
    Blend One Zero
    ZTest LEqual
    ZWrite On

    HLSLPROGRAM

    #pragma target 4.5
    #pragma exclude_renderers gles gles3 glcore
    #pragma multi_compile_instancing
    #pragma multi_compile _ DOTS_INSTANCING_ON
    #pragma vertex vert
    #pragma geometry Geometry
    #pragma fragment Fragment

    #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
    #pragma multi_compile _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS _ADDITIONAL_OFF
    #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
    #pragma multi_compile _ _SHADOWS_SOFT
    #pragma multi_compile _ SHADOWS_SHADOWMASK

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "LayeredGeo.hlsl"
#include "Vertex_Functions.hlsl"


    ENDHLSL
}
Pass
{
    Name "ShadowCaster"
    Tags
    {
        "LightMode" = "ShadowCaster"
    }
    Cull Back
    Blend One Zero
    ZTest LEqual
    ZWrite On

    HLSLPROGRAM

    #pragma target 4.5
    #pragma exclude_renderers gles gles3 glcore
    #pragma multi_compile_instancing
    #pragma multi_compile _ DOTS_INSTANCING_ON
    #pragma vertex vert
    #pragma geometry Geometry
    #pragma fragment Fragment

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "LayeredGeo.hlsl"
#include "Vertex_Functions.hlsl"

    ENDHLSL
}
Pass
{
    Name "DepthOnly"
    Tags
    {
        "LightMode" = "DepthOnly"
    }

    Cull Back
    Blend One Zero
    ZTest LEqual
    ZWrite On

    HLSLPROGRAM

    #pragma target 4.5
    #pragma exclude_renderers gles gles3 glcore
    #pragma multi_compile_instancing
    #pragma multi_compile _ DOTS_INSTANCING_ON
    #pragma vertex vert
    #pragma geometry Geometry
    #pragma fragment Fragment

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "LayeredGeo.hlsl"
#include "Vertex_Functions.hlsl"

    ENDHLSL
}
Pass
{
    Name "DepthNormals"
    Tags
    {
        "LightMode" = "DepthNormals"
    }

    Cull Back
    Blend One Zero
    ZTest LEqual
    ZWrite On

    HLSLPROGRAM

    #pragma target 4.5
    #pragma exclude_renderers gles gles3 glcore
    #pragma multi_compile_instancing
    #pragma multi_compile _ DOTS_INSTANCING_ON
    #pragma vertex vert
    #pragma geometry Geometry
    #pragma fragment Fragment

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "LayeredGeo.hlsl"
#include "Vertex_Functions.hlsl"

    ENDHLSL
}
    }
    FallBack "Hidden/Shader Graph/FallbackError"
}