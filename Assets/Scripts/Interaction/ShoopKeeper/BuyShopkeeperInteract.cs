using System.Collections;
using System.Collections.Generic;
using CC.Events;
using CC.Inventory;
using CC.Shop;
using UnityEngine;

namespace CC.Interaction.ShoopKeeper
{
    public class BuyShopkeeperInteract : MonoBehaviour, IInteraction
    {
        [SerializeField] private ItemShopkeeper _shopkeeperInventoryData;
        [SerializeField] private NPCInventoryDataChannel _onTriggerBuyEvent;

        // Start is called before the first frame update

        public void Interact()
        {
            ShowShopkeeper();
        }

        public void ShowShopkeeper()
        {
            _onTriggerBuyEvent.RaiseEvent(_shopkeeperInventoryData);
        }
    }
}
