using UnityEngine;
using UnityEditor;
using UnityEngine.TerrainTools;
using UnityEditor.TerrainTools;
using UnityEngine.SceneManagement;
using Unity.Mathematics;
using System.Linq;

public class ErraBrush : TerrainPaintTool<ErraBrush>
{
    Texture2D texts;
    RenderTexture RT_Render;

    
    
    public override string GetDescription()
    {
       return "Erra Tool";
    }

    public override void OnInspectorGUI(Terrain terrain, IOnInspectorGUI editContext)
    {
        //if (GUILayout.Button("TerrInfo"))
        //{
        //    Debug.Log(terrain.terrainData.size.z);
        //}

        if (GUILayout.Button("Render Terrain!"))
        {
            resetHeightmap(terrain, editContext);
            renderHeightmap(terrain, editContext);
        }

        //texts = (Texture2D)EditorGUILayout.ObjectField("Test Image", texts, typeof(Texture2D), false);

        //RT_Render = (RenderTexture)EditorGUILayout.ObjectField("RenderTex", RT_Render, typeof(RenderTexture), false);
        //TexRenderer = (Camera)EditorGUILayout.ObjectField("Test Image", TexRenderer, typeof(Camera), false);
        base.OnInspectorGUI(terrain, editContext);

        

    }


    private void renderHeightmap(Terrain terrain, IOnInspectorGUI editContext)
    {

        Rect renderedTextureRect = new Rect(0, 0, terrain.terrainData.baseMapResolution, terrain.terrainData.baseMapResolution);
        RenderTexture renderedTexture = new RenderTexture(terrain.terrainData.baseMapResolution, terrain.terrainData.baseMapResolution, 32);
        Texture2D heightmapTexture = new Texture2D(terrain.terrainData.baseMapResolution, terrain.terrainData.baseMapResolution, TextureFormat.RGBA32, false);

        Camera tempCamera = new Camera();
        
        ErraCamera _cam = (ErraCamera)GameObject.FindFirstObjectByType(typeof(ErraCamera));



        Camera cam = _cam.GetComponent<Camera>();
        cam.targetTexture = renderedTexture;
        cam.Render();
        cam.targetTexture = null;

        RenderTexture.active = renderedTexture;
        heightmapTexture.ReadPixels(renderedTextureRect, 0, 0);
        texts = heightmapTexture;
        texts.Apply();
        RenderTexture.active = null;


        //byte[] bytes;
        //bytes = texts.EncodeToPNG();

        //string path = SceneManager.GetActiveScene().path.Split('.')[0] + _cam.GetHashCode() + ".png";
        //System.IO.File.WriteAllBytes(path, bytes);
        //AssetDatabase.ImportAsset(path);

        //Debug.Log("SAVED TO " + path);



        //then finally apply it to the terrain
        Material brushMt = TerrainPaintUtility.GetBuiltinPaintMaterial();

        //BrushTransform brushTf = TerrainPaintUtility.CalculateBrushTransform(terrain, editContext.uv, editContext.brushSize, 0.0f);
        BrushTransform brushTf = BrushTransform.FromRect(new Rect(0, 0, terrain.terrainData.baseMapResolution, terrain.terrainData.baseMapResolution));
        PaintContext paintCtx = TerrainPaintUtility.BeginPaintHeightmap(terrain, brushTf.GetBrushXYBounds());

        //apply?
        Vector4 brushPrm = new Vector4(0.5f, 0.0f, 0.0f, 0.0f);
        brushMt.SetTexture("_BrushTex", texts);
        brushMt.SetVector("_BrushParams", brushPrm);
        TerrainPaintUtility.SetupTerrainToolMaterialProperties(paintCtx, brushTf, brushMt);

        Graphics.Blit(paintCtx.sourceRenderTexture, paintCtx.destinationRenderTexture, brushMt, (int)TerrainBuiltinPaintMaterialPasses.RaiseLowerHeight);

        TerrainPaintUtility.EndPaintHeightmap(paintCtx, "Erra Brush - Test");



        //////////////////////////////////////

        //tempCamera.orthographic = true;
        //tempCamera.orthographicSize = 500;
        //tempCamera.cullingMask = 6;
        //tempCamera.backgroundColor = Color.black;
        //Debug.Log(tempCamera.transform);

        //tempCamera.name = "tempTexRenderer";

        //Vector3 campos = new Vector3(500.0f, 1000.0f, 500.0f);
        //Instantiate(tempCamera);



        //Instantiate(tempCamera, campos , Quaternion.);



        //TexRenderer.targetTexture = renderedTexture;
        //TexRenderer.Render();

        //heightmapTexture.ReadPixels(renderedTextureRect, 0, 0);
        //heightmapTexture.Apply();

        //TexRenderer.targetTexture = null;
        //RenderTexture.active = null;

        //texts = heightmapTexture;

        //Destroy(tempCamera);

        //Camera tempCamera = Instantiate(new Camera, 0, 0;


    }



    private void resetHeightmap(Terrain terrain, IOnInspectorGUI editContext)
    {
        int terrX = terrain.terrainData.heightmapResolution;
        int terrY = terrain.terrainData.heightmapResolution;


        float[,] heights = terrain.terrainData.GetHeights(0, 0, terrX, terrY);

        for (int h = 0; h < terrX; h++)
        {
            for (int f = 0; f  < terrX; f++)
            {
                heights[h, f] = 0.0f;
            }
        }
        terrain.terrainData.SetHeights(0, 0, heights);
    }

    //public override void OnSceneGUI(Terrain terrain, IOnSceneGUI editContext)
    //{
    //    Texture2D textus;
    //    base.OnSceneGUI(terrain, editContext);
    //}

    public override string GetName()
    {
       return "Erra Tool";
    }

    public override void OnRenderBrushPreview(Terrain terrain, IOnSceneGUI editContext)
    {
        TerrainPaintUtilityEditor.ShowDefaultPreviewBrush(terrain, texts, editContext.brushSize);
        //TerrainPaintUtilityEditor.
        //base.OnRenderBrushPreview(terrain, editContext);
    }

    public override bool OnPaint(Terrain terrain, IOnPaint editContext)
    {
        Material brushMt = TerrainPaintUtility.GetBuiltinPaintMaterial();

        //BrushTransform brushTf = TerrainPaintUtility.CalculateBrushTransform(terrain, editContext.uv, editContext.brushSize, 0.0f);
        BrushTransform brushTf = BrushTransform.FromRect(new Rect(0, 0, terrain.terrainData.baseMapResolution, terrain.terrainData.baseMapResolution));
        PaintContext paintCtx = TerrainPaintUtility.BeginPaintHeightmap(terrain, brushTf.GetBrushXYBounds());

        //apply?
        Vector4 brushPrm = new Vector4(100, 0.0f, 0.0f, 0.0f);
        brushMt.SetTexture("_BrushTex",texts);
        brushMt.SetVector("_BrushParams", brushPrm);
        TerrainPaintUtility.SetupTerrainToolMaterialProperties(paintCtx, brushTf, brushMt);

        Graphics.Blit(paintCtx.sourceRenderTexture, paintCtx.destinationRenderTexture, brushMt, (int)TerrainBuiltinPaintMaterialPasses.RaiseLowerHeight);

        TerrainPaintUtility.EndPaintHeightmap(paintCtx, "Erra Brush - Test");
        return false;
    }
}
