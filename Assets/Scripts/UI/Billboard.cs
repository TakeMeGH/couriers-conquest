using UnityEngine;

namespace CC.UI
{
    public class Billboard : MonoBehaviour
    {
        UnityEngine.Camera cam;
        void Start()
        {
            cam = UnityEngine.Camera.main;
        }

        private void LateUpdate()
        {
            transform.LookAt(cam.transform.position);
        }
    }

}
