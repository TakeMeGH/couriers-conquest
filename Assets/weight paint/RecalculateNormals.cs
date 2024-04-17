using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC
{
    [RequireComponent(typeof(MeshFilter))]
    public class RecalculateNormals : MonoBehaviour
    {
        void Start()
        {
            var meshFilter = GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                meshFilter.mesh.RecalculateNormals();
            }
        }
    }
}
