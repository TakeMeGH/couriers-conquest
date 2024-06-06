using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Newtonsoft.Json.Linq;
using CC.Inventory;
using CC.Items;
using CC.Events;

namespace CC.Core.Save
{
    [CreateAssetMenu(menuName = "Data/Dynamics/PlayerInventory")]
    public class PlayerInventorySaveData : ASavableModel
    {
        [SerializeField] private SaveDataInventory _saveInventoryData;
        [SerializeField] private InventoryData _playerInventory;
        private int _currentIndex = 0;

        [Header("DefaultInventory")]
        [SerializeField] private SaveDataInventory _defaultInventoryData;
        [SerializeField] private ListItemID _listItemID;
        [SerializeField] private ItemInventoryEventChannel _addItemToInventory;
        public override void Load(object data)
        {
            _saveInventoryData = ((JObject)data).ToObject<SaveDataInventory>();

            AddToInventory();
            SetOtherValue();
        }

        private void SetOtherValue()
        {
            _playerInventory.playerGold = _saveInventoryData.playerGold;
            _playerInventory.isRuneEquiped = _saveInventoryData.isQuipedRune;
            _playerInventory.isPouchEquiped = _saveInventoryData.isEquipedPouch;
            _playerInventory.indexPouchEquiped = _saveInventoryData.indexPouch;
            _playerInventory.indexRuneEquiped = _saveInventoryData.indexRune;
        }

        public override ISaveable Save()
        {
            SaveInventory();
            return _saveInventoryData;
        }

        public override void SetDefaultValue()
        {
            _playerInventory.items.Clear();
            for (int i = 0; i < _playerInventory.inventorySize; i++)
            {
                _playerInventory.items.Add(new ItemSlotInfo(null, 0));
            }

            _saveInventoryData.CopyFrom(_defaultInventoryData);
            AddToInventory();
            SetOtherValue();
        }

        private void AddToInventory()
        {
            _currentIndex = 0;
            RefreshInventoryData();
            foreach (var kvp in _saveInventoryData.dataValue)
            {
                string key = kvp.key;
                int value = kvp.amount;
                float quality = kvp.quality;

                ABaseItem item = _listItemID.GetItemByID(key);
                if (item.GetItemType() == ItemType.QuestItem)
                {
                    ((QuestItem)item).CurrentQuality = quality;
                }
                Debug.Log("ID Item " + kvp.key + " : " + kvp.amount + " - " + item.itemName);
                OnAddItem(item, value);
            }
        }

        public void OnAddItem(ABaseItem item, int amount)
        {
            if (item != null)
            {
                foreach (ItemSlotInfo i in _playerInventory.items)
                {
                    if (i.item == null)
                    {
                        i.item = item;
                        i.stacks = amount;

                        if (i.item.GetItemType() == ItemType.Equipment)
                        {
                            EquipmentItem equipmentData = (EquipmentItem)i.item;
                            equipmentData.equipmentLevel = GetEquipmentLevel(i.item.idItem);
                        }
                        return;
                    }
                }
            }
        }

        private void SaveInventory()
        {
            _saveInventoryData.dataValue = new List<RawInventorySaveData>();
            _saveInventoryData.equipmentLevel = new SerializedDictionary<string, int>();
            SetInventorytData();
        }

        private void RefreshInventoryData()
        {
            foreach (ItemSlotInfo i in _playerInventory.items)
            {
                i.item = null;
                i.stacks = 0;
                i.name = string.Empty;
            }
        }

        public void SetInventorytData()
        {
            foreach (ItemSlotInfo i in _playerInventory.items)
            {
                if (i.item != null)
                {
                    RawInventorySaveData data = new();
                    if (i.item.GetItemType() == ItemType.Equipment)
                    {
                        EquipmentItem equipmentData = (EquipmentItem)i.item;
                        _saveInventoryData.equipmentLevel.Add(equipmentData.idItem, equipmentData.equipmentLevel);
                    }

                    data.key = i.item.idItem;
                    data.amount = i.stacks;
                    if (i.item.GetItemType() == ItemType.QuestItem)
                    {
                        data.quality = ((QuestItem)i.item).CurrentQuality;
                    }
                    _saveInventoryData.dataValue.Add(data);
                    Debug.Log("Add : " + i.item.itemName);
                }
            }

            _saveInventoryData.playerGold = _playerInventory.playerGold;
            _saveInventoryData.isEquipedPouch = _playerInventory.isPouchEquiped;
            _saveInventoryData.isQuipedRune = _playerInventory.isRuneEquiped;
            _saveInventoryData.indexPouch = _playerInventory.indexPouchEquiped;
            _saveInventoryData.indexRune = _playerInventory.indexRuneEquiped;

        }

        private int GetEquipmentLevel(string key)
        {
            if (_saveInventoryData.equipmentLevel.ContainsKey(key))
            {
                return _saveInventoryData.equipmentLevel[key];
            }
            else
            {
                return 0;
            }
        }
    }

    [System.Serializable]
    public class SaveDataInventory : ISaveable
    {
        public List<RawInventorySaveData> dataValue;
        public SerializedDictionary<string, int> equipmentLevel;
        public int playerGold;
        public bool isEquipedPouch;
        public bool isQuipedRune;
        public int indexPouch;
        public int indexRune;
        public void CopyFrom(ISaveable obj)
        {
            var target = (SaveDataInventory)obj;
            this.dataValue = new(target.dataValue);
            this.equipmentLevel = new(target.equipmentLevel);
            this.playerGold = target.playerGold;
            this.isEquipedPouch = target.isEquipedPouch;
            this.indexPouch = target.indexPouch;
            this.indexRune = target.indexRune;
        }
    }

    [System.Serializable]
    public class RawInventorySaveData
    {
        public string key;
        public int amount;
        public float quality;
    }
}
