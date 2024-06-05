using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CC.Inventory
{
    public class PouchAndRuneManager : MonoBehaviour
    {
        private PlayerInventoryManager _playerInventoryManager;
        private InventoryData _inventoryData;

        [SerializeField] private ABaseItem _pouchSlot;
        [SerializeField] private ABaseItem _runeSlot;

        [Header("UI COMPONENT")]
        [SerializeField] private Image _iconRune;
        [SerializeField] private Image bgRune;

        [SerializeField] private Image _iconPouch;
        [SerializeField] private Image _bgPouch;

        public void Initialize(PlayerInventoryManager playerInventoryManager, InventoryData inventoryData)
        {
            _playerInventoryManager = playerInventoryManager;
            _inventoryData = inventoryData;

            LoadLastPouch();
        }

        private void LoadLastPouch()
        {
            if (_inventoryData.isPouchEquiped)
            {
                EquipPouch(_inventoryData.items[_inventoryData.indexPouchEquiped].item, _inventoryData.indexPouchEquiped);
            }
        }

        public void EquipPouch(ABaseItem item, int targetIndex)
        {
            UpdatePouchIconEquip(targetIndex);
            _pouchSlot = item;
            _inventoryData.indexPouchEquiped = targetIndex;
            _inventoryData.isPouchEquiped = true;
            RefreshUIPouch();
        }

        public void UnEquipPouch()
        {
            _pouchSlot = null;
            _playerInventoryManager.existingPanels[_inventoryData.indexPouchEquiped].equipedPanel.SetActive(false);
            _inventoryData.isPouchEquiped = false;
            RefreshUIPouch();
        }

        private void UpdatePouchIconEquip(int targetIndex)
        {
            _playerInventoryManager.existingPanels[_inventoryData.indexPouchEquiped].equipedPanel.SetActive(false);
            _playerInventoryManager.existingPanels[targetIndex].equipedPanel.SetActive(true);
        }

        private void RefreshUIPouch()
        {
            if(_pouchSlot == null)
            {
                _iconPouch.enabled = false;
                _bgPouch.enabled = true;
            }
            else
            {
                _iconPouch.enabled = true;
                _bgPouch.enabled = false;
                _iconPouch.sprite = _pouchSlot.itemSprite;
            }

        }


    }
}
