using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Water_Settings : MonoBehaviour
{
    Material waterVolume;
    Material waterMaterial;

    void Update()
    {
        if(waterVolume == null)
        {
            waterVolume = (Material)Resources.Load("Water_Volume");
        }

        if (waterMaterial == null)
        {
            waterMaterial = GetComponent<MeshRenderer>().sharedMaterial;
        }

        waterVolume.SetVector("pos", new Vector4(0, (waterVolume.GetVector("bounds").y / -2) + transform.position.y + (waterMaterial.GetFloat("_Displacement_Amount") / 3), 0, 0));
    }
}
