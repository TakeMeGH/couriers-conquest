using CC.Core.Save;
using CC.Inventory;
using CC.Items;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CC.Core.Data.Dynamic
{
    [CreateAssetMenu(menuName = "Data/Dynamics/PlayerStates")]
    public class WorldChestDataModel : ASavableModel
    {
        [SerializeField] private SaveDataWorldChest _saveWorldChestData;

        [Header("Default World Chest")]
        [SerializeField] private SaveDataWorldChest _defaultWorldChestData;

        public SaveDataWorldChest saveWorldChestData { get => _saveWorldChestData;}

        public override void Load(object data)
        {
            _saveWorldChestData = ((JObject)data).ToObject<SaveDataWorldChest>();
        }

        public override ISaveable Save()
        {
            return _saveWorldChestData;
        }

        public override void SetDefaultValue()
        {
            ChestData[] chestData = Resources.LoadAll<ChestData>("Chest");

            _defaultWorldChestData.dataValue.Clear();

            foreach (ChestData item in chestData)
            {
                if (item != null)
                {
                    item.isColectable = false;
                    _defaultWorldChestData.dataValue.Add(item.isColectable);
                }
            }

            _saveWorldChestData.CopyFrom(_defaultWorldChestData);
        }
    }

    public class SaveDataWorldChest : ISaveable
    {
        public List<bool> dataValue;
        public void CopyFrom(ISaveable obj)
        {
            var target = (SaveDataWorldChest)obj;
            this.dataValue = new(target.dataValue);
        }
    }
}
