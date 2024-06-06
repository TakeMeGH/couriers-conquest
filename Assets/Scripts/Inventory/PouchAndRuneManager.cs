using CC.Core.Data.Dynamic;
using CC.Core.Data.Stable;
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
        private PlayerStatsSO _playerStats;
        private ItemsActionPlayerStats _actionPlayerStats;
        private StatsModifier _activeRuneModifier;
        [SerializeField] private InputReader _inputReader;

        [SerializeField] private ABaseItem _pouchSlot;
        [SerializeField] private ABaseItem _runeSlot;

        [Header("UI COMPONENT")]
        [SerializeField] private Image _iconRune;
        [SerializeField] private Image _bgRune;

        [SerializeField] private Image _iconPouch;
        [SerializeField] private Image _bgPouch;

        private void OnEnable()
        {
            _inputReader.PouchPerformed += AttempToUsePouch;
            _actionPlayerStats = GetComponent<ItemsActionPlayerStats>();
            _actionPlayerStats.Initialize(_inventoryData);
        }

        private void OnDisable()
        {
            _inputReader.PouchPerformed -= AttempToUsePouch;
        }

        public void Initialize(PlayerInventoryManager playerInventoryManager, InventoryData inventoryData, PlayerStatsSO playerstats)
        {
            _playerInventoryManager = playerInventoryManager;
            _inventoryData = inventoryData;
            _playerStats = playerstats;
            LoadLastPouch();
        }

        private void LoadLastPouch()
        {
            if (_inventoryData.isPouchEquiped)
            {
                EquipPouch(_inventoryData.items[_inventoryData.indexPouchEquiped].item, _inventoryData.indexPouchEquiped);
            }

            if (_inventoryData.isRuneEquiped)
            {
                EquipRune(_inventoryData.items[_inventoryData.indexRuneEquiped].item, _inventoryData.indexRuneEquiped);
                RuneItem item = (RuneItem)_inventoryData.items[_inventoryData.indexRuneEquiped].item;
                _playerStats.ForceAddModifier(item.GetRuneStats());
                _activeRuneModifier = item.GetRuneStats();
            }
        }

        #region Pouch Manager

        public void EquipPouch(ABaseItem item, int targetIndex)
        {
            UpdatePouchIconEquip(targetIndex);
            _pouchSlot = item;
            _inventoryData.indexPouchEquiped = targetIndex;
            _inventoryData.isPouchEquiped = true;
            RefreshUIPouch();

            //Here To Modify Player Stats
        }

        public void UnEquipPouch()
        {
            _pouchSlot = null;
            _playerInventoryManager.existingPanels[_inventoryData.indexPouchEquiped].equipedPanel.SetActive(false);
            _inventoryData.isPouchEquiped = false;
            RefreshUIPouch();

            //Here To Modify Player Stats
        }

        private void UpdatePouchIconEquip(int targetIndex)
        {
            _playerInventoryManager.existingPanels[_inventoryData.indexPouchEquiped].equipedPanel.SetActive(false);
            _playerInventoryManager.existingPanels[targetIndex].equipedPanel.SetActive(true);
        }

        private void HidePouchIcon(int targetIndex)
        {
            _playerInventoryManager.existingPanels[_inventoryData.indexPouchEquiped].equipedPanel.SetActive(false);
            _playerInventoryManager.existingPanels[targetIndex].equipedPanel.SetActive(false);
        }

        private void RefreshUIPouch()
        {
            if (_pouchSlot == null)
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

        #endregion

        #region Rune Manager

        public void EquipRune(ABaseItem item, int targetIndex)
        {
            UpdateRuneIconEquip(targetIndex);
            _runeSlot = item;
            _inventoryData.indexRuneEquiped = targetIndex;
            _inventoryData.isRuneEquiped = true;

            if (_runeSlot != null)
            {
                _playerStats.RemoveModifier(_activeRuneModifier);
            }

            RuneItem itemRune = (RuneItem)_inventoryData.items[_inventoryData.indexRuneEquiped].item;
            _playerStats.ForceAddModifier(itemRune.GetRuneStats());
            _activeRuneModifier = itemRune.GetRuneStats();

            RefreshUIRune();
            _playerInventoryManager.UpdatePlayerStatus();
        }

        public void UnEquipRune()
        {
            _runeSlot = null;
            _playerInventoryManager.existingPanels[_inventoryData.indexRuneEquiped].equipedPanel.SetActive(false);
            _inventoryData.isRuneEquiped = false;

            _playerStats.RemoveModifier(_activeRuneModifier);
            RefreshUIRune();
            _playerInventoryManager.UpdatePlayerStatus();
        }

        private void UpdateRuneIconEquip(int targetIndex)
        {
            _playerInventoryManager.existingPanels[_inventoryData.indexRuneEquiped].equipedPanel.SetActive(false);
            _playerInventoryManager.existingPanels[targetIndex].equipedPanel.SetActive(true);
        }

        private void HideRuneIcon(int targetIndex)
        {
            _playerInventoryManager.existingPanels[_inventoryData.indexRuneEquiped].equipedPanel.SetActive(false);
            _playerInventoryManager.existingPanels[targetIndex].equipedPanel.SetActive(false);
        }

        private void RefreshUIRune()
        {
            if (_runeSlot == null)
            {
                _iconRune.enabled = false;
                _bgRune.enabled = true;
            }
            else
            {
                _iconRune.enabled = true;
                _bgRune.enabled = false;
                _iconRune.sprite = _runeSlot.itemSprite;
            }
        }

        #endregion

        private void AttempToUsePouch()
        {
            if (!_inventoryData.isPouchEquiped) return; ;

            int indexPouch = _inventoryData.indexPouchEquiped;
            ABaseItem item = _inventoryData.items[indexPouch].item;
            if (item != null && _inventoryData.items[indexPouch].stacks > 0)
            {
                ConsumableItem consumableItem = (ConsumableItem)item;
                CheckItemEffectType(consumableItem, indexPouch);
                ReduceItem(indexPouch, true);
                _playerInventoryManager.UpdatePlayerStatus();
            }
            else
            {
                Debug.Log("Empty Pouch");
            }
        }

        public void AttempToConsumeItem(int indexItem)
        {
            ABaseItem item = _inventoryData.items[indexItem].item;
            ConsumableItem consumableItem = (ConsumableItem)item;
            CheckItemEffectType(consumableItem, indexItem);
            ReduceItem(indexItem, false);
            _playerInventoryManager.UpdatePlayerStatus();
        }

        private void CheckItemEffectType(ConsumableItem item, int index)
        {
            if (item.GetConsumableType() == ConsumableType.RegenerationHP)
            {
                _actionPlayerStats.AttempToOvertimeRegeneration(item.GetAmount(mainStat.Health), item.DurationEffect(), mainStat.Health);
                _playerInventoryManager.RefreshInventory();
            }
            else if (item.GetConsumableType() == ConsumableType.IncreaseStamina)
            {
                _actionPlayerStats.AttempToOvertimeRegeneration(item.GetAmount(mainStat.Stamina), item.DurationEffect(), mainStat.Stamina);
                _playerInventoryManager.RefreshInventory();
            }
            else if (item.GetConsumableType() == ConsumableType.IncreaseAttack)
            {
                _actionPlayerStats.AttempToIncreaseStat(item.GetAmount(mainStat.AttackValue), item.DurationEffect(), mainStat.AttackValue);
                _playerInventoryManager.RefreshInventory();
            }
            else if (item.GetConsumableType() == ConsumableType.IncreaseDefense)
            {
                _actionPlayerStats.AttempToIncreaseStat(item.GetAmount(mainStat.Defense), item.DurationEffect(), mainStat.Defense);
                _playerInventoryManager.RefreshInventory();
            }
            else if (item.GetConsumableType() == ConsumableType.IncreaseSpeed)
            {
                _actionPlayerStats.AttempToIncreaseStat(item.GetAmount(mainStat.MovementSpeed), item.DurationEffect(), mainStat.MovementSpeed);
                _playerInventoryManager.RefreshInventory();
            }
        }

        private void ReduceItem(int index, bool isPouch)
        {
            _inventoryData.items[index].stacks--;
            if (_inventoryData.items[index].stacks <= 0)
            {
                _inventoryData.items[index].item = null;

                if (isPouch)
                {
                    _inventoryData.isPouchEquiped = false;
                    UnEquipPouch();
                }
                else
                {
                    _playerInventoryManager.ChangeFrameToDefault(index);
                    _playerInventoryManager.activeSlot = ItemType.None;
                    _playerInventoryManager.SetNoActionLabel();
                }
            }

            _playerInventoryManager.RefreshInventory();
        }
    }
}
