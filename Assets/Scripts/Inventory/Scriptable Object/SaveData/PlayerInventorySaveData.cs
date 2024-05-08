using CC.Core.Data.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CC.Core.Save;
using AYellowpaper.SerializedCollections;
using Newtonsoft.Json.Linq;
using CC.Inventory;
using CC.Items;
using System;

namespace CC.Core.Save
{
    [CreateAssetMenu(menuName = "Data/Dynamics/PlayerInventory")]
    public class PlayerInventorySaveData : ASavableModel
    {
        [SerializeField] private SaveDataInventory _saveInventoryData;
        [SerializeField] private InventoryData _playerInventory;
        [SerializeField] private ListItemID _listItemID;
        public override void Load(object data)
        {
            _saveInventoryData = ((JObject)data).ToObject<SaveDataInventory>();

            foreach (var kvp in _saveInventoryData.dataValue)
            {
                string key = kvp.Key;
                int value = kvp.Value;

                Debug.Log(_listItemID.GetItemByID(key).itemName);
            }
        }

        public override ISaveable Save()
        {
            return _saveInventoryData;
        }

        public override void SetDefaultValue()
        {
            _saveInventoryData.CopyFrom(_saveInventoryData);
        }
    }


    [System.Serializable]
    public class SaveDataInventory : ISaveable
    {
        public SerializedDictionary<string, int> dataValue;
        public SerializedDictionary<string, int> defaultValue;
        public void CopyFrom(ISaveable obj)
        {
            var target = (SaveDataInventory)obj;
            this.defaultValue = new(target.defaultValue);
            this.dataValue = new(target.dataValue);
        }
    }
}
