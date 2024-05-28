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
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TextMeshProUGUI _textStack;
        [SerializeField] private GameObject _pouchPanel;

        private ItemsActionPlayerStats _playerStatItemsAction;

        void Update()
        {
            // TODO : Update health must be called from Player Health Controller
            UpdateHealthUI();
        }
        public void Initialize(PlayerInventoryManager playerInventoryManaer, InventoryData _inventoryData)
        {
            _playerInventoryManager = playerInventoryManaer;
            _playerInventoryData = _inventoryData;
            _playerInventoryData.inputReader.PouchPerformed += OnPouchAction;

            _playerStatItemsAction = GetComponent<ItemsActionPlayerStats>();
            _playerStatItemsAction.Initialize(this, _playerStats, _playerInventoryData);
            SetDefaultStats();
            //UpdatePouchUI();
        }

        private void OnDisable()
        {
            _playerInventoryData.inputReader.PouchPerformed -= OnPouchAction;
        }

        private void OnPouchAction()
        {
            AttempToUsePouch(_playerInventoryData.indexPouchEquiped);
        }

        private void SetDefaultStats()
        {
            _healthSlider.maxValue = _playerStats.GetValue(mainStat.Health);
            _healthSlider.value = _playerStats.GetInstanceValue(mainStat.Health);
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
        public void UpdateHealthUI()
        {
            _healthSlider.value = _playerStats.GetInstanceValue(mainStat.Health);
        }

        /*public void UpdatePouchUI()
        {
            if (_playerInventoryData.items[_playerInventoryData.pouchIndex].item != null && _playerInventoryData.items[_playerInventoryData.pouchIndex].stacks > 0)
            {
                PouchUI(true);
                _itemIcon.sprite = _playerInventoryData.items[_playerInventoryData.pouchIndex].item.itemSprite;
                _textStack.text = _playerInventoryData.items[_playerInventoryData.pouchIndex].stacks.ToString();
            }
            else
            {
                PouchUI(false);
            }
        }*/

        private void PouchUI(bool condition)
        {
            _itemIcon.gameObject.SetActive(condition);
            _textStack.gameObject.SetActive(condition);
        }
        public void ShowPouchPanel()
        {
            _pouchPanel.SetActive(true);
            //UpdatePouchUI();
        }

        public void HidePouchPanel()
        {
            _pouchPanel.SetActive(false);
            //UpdatePouchUI();
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
