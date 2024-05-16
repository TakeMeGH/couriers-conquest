using System.Collections;
using System.Collections.Generic;
using CC.Events;
using CC.Inventory;
using UnityEngine;

namespace CC.Interaction.ShoopKeeper
{
    public class ShoopKeeperInteract : MonoBehaviour, IInteraction
    {

        [SerializeField] private AInventoryData _playerInventoryData;
        [SerializeField] private AInventoryData _shopkeeperInventoryData;
        [SerializeField] private PlayerInventoryDataChannel _shopEvent;
        // Start is called before the first frame update

        public void Interact()
        {
            ShowShopkeeper();
        }

        public void ShowShopkeeper()
        {
            _shopEvent.RaiseEvent(_playerInventoryData, _shopkeeperInventoryData);
        }
    }
}
