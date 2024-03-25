using CC.Interaction;
using UnityEngine;

namespace CC.Example.Interaction
{
    public class NPCExample : MonoBehaviour, IInteraction
    {
        public void Interact()
        {
            Debug.Log("NPC DIALOGUE");
        }
    }

}
