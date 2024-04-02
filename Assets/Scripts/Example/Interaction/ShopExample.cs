using CC.Interaction;
using UnityEngine;

namespace CC.Example.Interaction
{
    public class ShopExample : MonoBehaviour, IInteraction
    {
        public void Interact()
        {
            Debug.Log("SHOP");
        }

    }
}