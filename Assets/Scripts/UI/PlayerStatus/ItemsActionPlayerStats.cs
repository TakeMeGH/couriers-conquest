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
        private UIPlayerStatus _uiPlayerStats;
        private InventoryData _playerInventoryData;

        private float amountRegeneration;

        public void Initialize(UIPlayerStatus uiPlayerStatus, PlayerStatsSO playerStats, InventoryData playerInventoryData)
        {
            _uiPlayerStats = uiPlayerStatus;
            _playerStats = playerStats;
            _playerInventoryData = playerInventoryData;
        }

        public void AttempToOvertimeRegeneration(float regenerationPoint, float regenDuration)
        {
            amountRegeneration = regenerationPoint;
            InvokeRepeating("StartOvertimeRegenerationHP", 0f, 1f);
            Invoke("StopOvetimeRegenerationHP", regenDuration);
            ReduceItem();
        }

        public void StartOvertimeRegenerationHP()
        {
            float _newPlayerHP = _playerStats.GetInstanceValue(mainStat.Health);
            float _maxHp = _playerStats.GetValue(mainStat.Health);
            _playerStats.SetInstanceValue(mainStat.Health, _newPlayerHP + _maxHp);
            _uiPlayerStats.UpdateHealthUI();
        }

        public void StopOvetimeRegenerationHP()
        {
            CancelInvoke("StartOvertimeRegenerationHP");
        }

        private void ReduceItem()
        {
            _playerInventoryData.items[_playerInventoryData.pouchIndex].stacks--;
            if (_playerInventoryData.items[_playerInventoryData.pouchIndex].stacks <= 0)
            {
                _playerInventoryData.items[_playerInventoryData.pouchIndex].item = null;
            }
            _uiPlayerStats.UpdatePouchUI();
        }
    }
}
