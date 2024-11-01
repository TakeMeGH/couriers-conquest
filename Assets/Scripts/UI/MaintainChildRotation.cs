using UnityEngine;

namespace CC
{
    public class MaintainChildRotation : MonoBehaviour
    {
        [SerializeField] Transform parentObject;
        [SerializeField] Transform childObject;
        private Quaternion initialChildRotation;

        void Start()
        {
            initialChildRotation = childObject.localRotation;
        }

        void Update()
        {
            childObject.localRotation = Quaternion.Inverse(parentObject.localRotation) * initialChildRotation;
        }
    }

}
