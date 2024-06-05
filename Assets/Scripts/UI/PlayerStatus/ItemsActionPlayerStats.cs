using CC.Core.Data.Dynamic;
using CC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    public class ItemsActionPlayerStats : MonoBehaviour
    {
        private PlayerStatsSO _playerStats;
        private InventoryData _playerInventoryData;

        private float amountRegeneration;

        public void Initialize(PlayerStatsSO playerStats, InventoryData playerInventoryData)
        {
            _playerStats = playerStats;
            _playerInventoryData = playerInventoryData;
        }

        public void AttempToOvertimeRegeneration(float regenerationPoint, float regenDuration, int index)
        {
            amountRegeneration = regenerationPoint;
            InvokeRepeating("StartOvertimeRegenerationHP", 0f, 1f);
            Invoke("StopOvetimeRegenerationHP", regenDuration);
            ReduceItem(index);
        }

        public void StartOvertimeRegenerationHP()
        {
            float _newPlayerHP = _playerStats.GetInstanceValue(mainStat.Health);
            float _maxHp = _playerStats.GetValue(mainStat.Health);
            _playerStats.SetInstanceValue(mainStat.Health, _newPlayerHP + _maxHp);

            Debug.Log("Regen : " + amountRegeneration.ToString());
        }

        public void StopOvetimeRegenerationHP()
        {
            CancelInvoke("StartOvertimeRegenerationHP");
        }

        private void ReduceItem(int index)
        {
            _playerInventoryData.items[index].stacks--;
            if (_playerInventoryData.items[index].stacks <= 0)
            {
                _playerInventoryData.items[index].item = null;
            }
            //_uiPlayerStats.UpdatePouchUI();
        }
    }
}
