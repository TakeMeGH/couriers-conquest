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
        [SerializeField] private InventoryDataEventChannel _shopEvent;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

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
