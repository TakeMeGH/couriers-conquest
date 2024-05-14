using CC.Events;
using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Interaction.Blacksmith
{
    public class BlacksmithInteract : MonoBehaviour
    {
        [SerializeField] private AInventoryData _playerInventoryData;
        [SerializeField] private AInventoryData _blacksmithInventoryData;
        [SerializeField] private PlayerInventoryDataChannel _upgradeEvent;

        public void Interact()
        {
            ShowShopkeeper();
        }

        public void ShowShopkeeper()
        {
            _upgradeEvent.RaiseEvent(_playerInventoryData, _blacksmithInventoryData);
        }
    }
}
