using CC.Core.Save;
using CC.Inventory;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Items
{
    [CreateAssetMenu(fileName = "Chest", menuName = "Items/Chest", order = 7)]
    public class ChestDataModel : ASavableModel
    {
        public List<ChestValue> itemChest = new List<ChestValue>();
        public int Gold;
        public ChestData CurrentChestData;
        public ChestData DefaultChestData;

        public override ISaveable Save()
        {
            return CurrentChestData;
        }

        public override void Load(object data)
        {
            CurrentChestData = ((JObject)data).ToObject<ChestData>();
        }

        public override void SetDefaultValue()
        {
            CurrentChestData.CopyFrom(DefaultChestData);
        }
    }

    [System.Serializable]
    public class ChestData : ISaveable
    {
        public bool IsAlreadyOpen;
        public void CopyFrom(ISaveable obj)
        {
            var target = (ChestData)obj;
            this.IsAlreadyOpen = target.IsAlreadyOpen;
        }
    }


    [System.Serializable]
    public class ChestValue
    {
        public ABaseItem item;
        public int amount;
    }
}
