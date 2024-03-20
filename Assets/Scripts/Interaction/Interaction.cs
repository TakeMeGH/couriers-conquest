using UnityEngine;

namespace CC.Interaction
{
    public class Interaction
    {
        public InteractionType type { get; private set; }
        public GameObject interactableObject { get; private set; }

        public Interaction(GameObject obj)
        {
            if (obj.CompareTag("Pickable"))
            {
                type = InteractionType.PickUp;
            }
            else if (obj.CompareTag("ShopKeeper"))
            {
                type = InteractionType.Shop;
            }
            else if (obj.CompareTag("NPC"))
            {
                type = InteractionType.Talk;
            }
            else
            {
                type = InteractionType.None;
            }

            interactableObject = obj;
        }
    }
}

