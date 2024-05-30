using CC.Core.Data.Dynamic;
using CC.Core.Data.Stable;
using CC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    public class ItemsActionPlayerStats : MonoBehaviour
    {
        [SerializeField] private PlayerStatsSO _playerStats;
        private InventoryData _inventoryData;

        private float _amountRegenerationHP;

        public void Initialize(InventoryData playerInventoryData)
        {
            _inventoryData = playerInventoryData;
        }

        public void AttempToOvertimeRegeneration(float regenerationPoint, float regenDuration, mainStat statType)
        {
            StartCoroutine(OvertimeRegenerationCoroutine(regenerationPoint, regenDuration, statType));
        }

        private IEnumerator OvertimeRegenerationCoroutine(float regenerationPoint, float regenDuration, mainStat statType)
        {
            float elapsed = 0f;

            while (elapsed < regenDuration)
            {
                StartOvertimeRegenerationHP(regenerationPoint, statType);
                yield return new WaitForSeconds(1f);
                elapsed += 1f;
            }
        }

        public void StartOvertimeRegenerationHP(float amount, mainStat statType)
        {
            float value = amount;
            float _currentAmount = _playerStats.GetInstanceValue(statType);
            float _maxAmount = _playerStats.GetValue(statType);

            if (_currentAmount + amount >= _maxAmount)
            {
                value = _maxAmount - _currentAmount;
            }

            _playerStats.SetInstanceValue(statType, _currentAmount + value);

            Debug.Log("Regen " + statType.ToString() + " : " + amount.ToString());
        }

        public void AttempToIncreaseStat(float amountPoint, float duration, mainStat statType)
        {
            Debug.Log("Increase");
            StartCoroutine(StatIncreaseCoroutine(amountPoint, duration, statType));
        }

        private IEnumerator StatIncreaseCoroutine(float amountPoint, float duration, mainStat statType)
        {
            float elapsed = 0f;
            StatsModifier _newModifier = IncreasePlayerStats(amountPoint, statType);

            while (elapsed < duration)
            {
                yield return new WaitForSeconds(1f);
                elapsed += 1f;
            }

            DecreasePlayerStats(_newModifier);
        }

        private StatsModifier IncreasePlayerStats(float amount, mainStat statType)
        {
            return _playerStats.AddModifier(statType, amount);
        }

        private void DecreasePlayerStats(StatsModifier modifier)
        {
            _playerStats.RemoveModifier(modifier);
        }
    }
}
