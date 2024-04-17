using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC
{
    public class AdjustMaterialProperties : MonoBehaviour
    {
        void Start()
        {
            var renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.SetFloat("_CullMode", 0); // 0 for off, 2 for backface culling
            }
        }
    }
}
