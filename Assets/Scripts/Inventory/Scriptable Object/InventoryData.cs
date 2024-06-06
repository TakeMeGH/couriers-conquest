using CC.Event;
using CC.Events;
using CC.UpgradeEquipment;
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
        public int inventoryIndex = 16;


        [SerializeField] public bool isRuneEquiped = false;
        [SerializeField] public bool isPouchEquiped = false;
        [SerializeField] public int indexRuneEquiped;
        [SerializeField] public int indexPouchEquiped;


        [Space]
        [Header("Event System")]
        [SerializeField] private ItemInventoryEventChannel _addItemToInventory;
        [SerializeField] private SenderDataEventChannelSO _removeItemEvent;
        [SerializeField] private ItemInventoryCheckEventChannel _itemCheckEvent;
        [SerializeField] private FloatEventChannelSO _onWeightUpdated;
        [SerializeField] private VoidEventChannelSO _onItemPickup;
        [SerializeField] private SellItemEventChannel _onSellItem;
        [SerializeField] private OnUpdateCurrencyEventChannel _onUpdateCurrency;
        [SerializeField] private OnSenderBaseItemEventChannel _onUpgradeEquipment;
        [SerializeField] private VoidEventChannelSO _onCharacterDamaged;
        [SerializeField] private SenderDataEventChannelSO _itemPickupUI;


        public InputReader inputReader { get => _inputReader; }
        public ItemInventoryEventChannel addItemToInventory { get => _addItemToInventory; }
        public SenderDataEventChannelSO removeItemEvent { get => _removeItemEvent; }
        public ItemInventoryCheckEventChannel itemCheckEvent { get => _itemCheckEvent; }
        public FloatEventChannelSO onWeightUpdated { get => _onWeightUpdated; }
        public VoidEventChannelSO onItemPickup { get => _onItemPickup; }
        public SellItemEventChannel onSellItem { get => _onSellItem; }
        public OnUpdateCurrencyEventChannel onUpdateCurrency { get => _onUpdateCurrency; }
        public OnSenderBaseItemEventChannel onUpgradeEquipment { get => _onUpgradeEquipment; }
        public VoidEventChannelSO onCharacterDamaged { get => _onCharacterDamaged; }
        public SenderDataEventChannelSO itemPickupUI { get => _itemPickupUI; }

        [Header("Player Currency")]
        [SerializeField] private int _playerGold;
        [field: SerializeField] public Sprite GoldSprite { get; private set; }

        public int playerGold
        {
            get => _playerGold;
            set => _playerGold = value;
        }


    }
}
