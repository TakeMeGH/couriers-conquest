using CC.Core.Data.Dynamic;
using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CC.UI
{
    public class UIPlayerStatus : MonoBehaviour
    {
        [Space]
        [SerializeField] private PlayerStatsSO _playerStats;
        private InventoryData _playerInventoryData;
        private PlayerInventoryManager _playerInventoryManager;

        [Header("UI Component")]
        [SerializeField] private GameObject _pouchPanel;
        private ItemsActionPlayerStats _playerStatItemsAction;
        public void Initialize(PlayerInventoryManager playerInventoryManaer, InventoryData _inventoryData)
        {
            _playerInventoryManager = playerInventoryManaer;
            _playerInventoryData = _inventoryData;
            _playerInventoryData.inputReader.PouchPerformed += OnPouchAction;

            _playerStatItemsAction = GetComponent<ItemsActionPlayerStats>();
            _playerStatItemsAction.Initialize(_playerStats, _playerInventoryData);
        }

        private void OnDisable()
        {
            _playerInventoryData.inputReader.PouchPerformed -= OnPouchAction;
        }

        private void OnPouchAction()
        {
            AttempToUsePouch(_playerInventoryData.indexPouchEquiped);
        }

        public void AttempToUsePouch(int targetIndex)
        {

            if (!_playerInventoryData.isPouchEquiped) return; ;

            ABaseItem item = _playerInventoryData.items[targetIndex].item;
            if (item != null && _playerInventoryData.items[targetIndex].stacks > 0)
            {
                ConsumableItem consumableItem = (ConsumableItem)item;
                CheckItemEffectType(consumableItem, targetIndex);
            }
            else
            {
                Debug.Log("Empty Pouch");
            }
        }

        public void ShowPouchPanel()
        {
            _pouchPanel.SetActive(true);
        }

        public void HidePouchPanel()
        {
            _pouchPanel.SetActive(false);
        }

        private void CheckItemEffectType(ConsumableItem item, int index)
        {
            if (item.GetConsumableType() == ConsumableType.HPRegeneration)
            {
                _playerStatItemsAction.AttempToOvertimeRegeneration(item.GetAmount(), item.DurationEffect(), index);
                _playerInventoryManager.RefreshInventory();
            }
        }
    }
}
