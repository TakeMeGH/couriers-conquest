using CC.Interaction;
using UnityEngine;

public class ItemExample : MonoBehaviour, IInteraction
{
    public void Interact()
    {
        Debug.Log("PICKUP ITEM");
    }

}
