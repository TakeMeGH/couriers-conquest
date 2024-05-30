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
            //_playerStatItemsAction.Initialize(this, _playerStats, _playerInventoryData);
            SetDefaultStats();
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

            
        }

        public void UpdateHealthUI()
        {
            _healthSlider.value = _playerStats.GetInstanceValue(mainStat.Health);
        }

        
    }
}
