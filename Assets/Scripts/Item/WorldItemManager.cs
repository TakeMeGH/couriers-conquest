using CC.Core.Data.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Items
{
    [CreateAssetMenu(fileName = "EquipmentItem", menuName = "Items/Chest", order = 3)]
    public class WorldItemManager : MonoBehaviour
    {
        [SerializeField] private WorldChestDataModel _worldChestData;
        [SerializeField] private GameObject _panelChest;

        private int _amountData = 0;

        private void Start()
        {
            ChestTreasure[] allScriptsChest = GetComponentsInChildren<ChestTreasure>();
            _amountData = _worldChestData.saveWorldChestData.dataValue.Count;
            // Panggil metode tertentu pada masing-masing script
            for (int i = 0; i < _amountData; i++)
            {
                allScriptsChest[i].items.isColectable = _worldChestData.saveWorldChestData.dataValue[i];
                allScriptsChest[i].Initialize();
            }
        }
    }
}
