using CC.Event;
using CC.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    [CreateAssetMenu(fileName = "InventoryData", menuName = "Inventory/Player", order = 1)]
    public class InventoryData : AInventoryData
    {
        public float dropSpeed = 5;
        [SerializeField] private InputReader _inputReader;

        [Space]
        [Header("Event System")]
        [SerializeField] private ItemInventoryEventChannel _addItemToInventory;
        [SerializeField] private SenderDataEventChannelSO _removeItemEvent;
        [SerializeField] private ItemInventoryCheckEventChannel _itemCheckEvent;
        [SerializeField] private FloatEventChannelSO _onWeightUpdated;
        [SerializeField] private VoidEventChannelSO _onItemPickup;
        [SerializeField] private SellItemEventChannel _onSellItem;


        public InputReader inputReader { get => _inputReader; }
        public ItemInventoryEventChannel addItemToInventory { get => _addItemToInventory; }
        public SenderDataEventChannelSO removeItemEvent { get => _removeItemEvent; }
        public ItemInventoryCheckEventChannel itemCheckEvent {  get => _itemCheckEvent; }
        public FloatEventChannelSO onWeightUpdated { get => _onWeightUpdated; }
        public VoidEventChannelSO onItemPickup { get => _onItemPickup; }
        public SellItemEventChannel onSellItem { get => _onSellItem; }

    }
}
