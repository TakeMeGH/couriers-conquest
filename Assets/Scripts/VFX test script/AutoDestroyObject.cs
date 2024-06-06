using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC
{
    public class AutoDestroyObject : MonoBehaviour
    {

        [SerializeField] float _aliveDuration = 2f;
        // Update is called once per frame
        void Update()
        {
            _aliveDuration -= Time.deltaTime;
            if (_aliveDuration <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
