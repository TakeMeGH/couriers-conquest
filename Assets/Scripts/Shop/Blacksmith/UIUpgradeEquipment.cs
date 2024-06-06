using CC.Inventory;
using CC.Core.Data.Dynamic;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CC.Events;

namespace CC.UpgradeEquipment
{
    public class UIUpgradeEquipment : MonoBehaviour
    {
        private BlacksmithManager _blacksmithManager;
        private InventoryData _inventoryData;
        [SerializeField] private InputReader _inputReader;

        [Space]
        [Header("UI COMPONENT")]
        [SerializeField] private Image _itemBeforeImage;
        [SerializeField] private Image _itemAfterImage;

        [SerializeField] private TextMeshProUGUI _textLabelBefore;
        [SerializeField] private TextMeshProUGUI _textLabelAfter;
        [SerializeField] private TextMeshProUGUI _textStatBefore;
        [SerializeField] private TextMeshProUGUI _textStatAfter;
        [SerializeField] private TextMeshProUGUI _textPlayerGold;
        [SerializeField] private TextMeshProUGUI _textPrice;
        [SerializeField] Image _upgradePanelBackground;
        [SerializeField] Image _upgradePanelIcon;


        [Header("PANELS FAILED")]
        [SerializeField] private GameObject _panelsFailedProces;
        [SerializeField] private TextMeshProUGUI _textNotificationFailed;

        [SerializeField] private GameObject _panelUpgradeMaterials;
        [SerializeField] private GameObject _panelConfirm;

        [SerializeField] private OnUpdateCurrencyEventChannel _updateCurrencyEventChannel;
        [SerializeField] private SellItemEventChannel _onRemoveItemEvent;
        [SerializeField] private OnUpdateCurrencyEventChannel _onUpdateCurrency;
        [SerializeField] private OnSenderBaseItemEventChannel _onUpgradeEquipment;


        [SerializeField] private List<PanelMaterialsUpgrade> _panelsMaterialsRequiriment = new List<PanelMaterialsUpgrade>();
        [Header("Failed Sprite")]
        [SerializeField] Sprite _cannotUpgradeBackground;
        [SerializeField] Sprite _canUpgradeBackground;
        [SerializeField] Sprite _cannotUpgradeIcon;
        [SerializeField] Sprite _canUpgradeIcon;


        private EquipmentItem _items;
        private bool _hasSelected = false;
        private bool _hasSelectedUI = false;
        private bool _isMax = false;

        private void OnEnable()
        {
            _hasSelected = false;
            _inputReader.ConfirmActionPerformed += ShowPanelConfirm;
            _inputReader.BackActionPerformed += BackAction;
            _inputReader.EnableInventoryUIInput();
        }

        private void OnDisable()
        {
            _hasSelected = false;
            _inputReader.ConfirmActionPerformed -= ShowPanelConfirm;
            _inputReader.BackActionPerformed -= BackAction;
            _inputReader.EnableGameplayInput();
        }

        public void Initialize(BlacksmithManager blacksmithManager, InventoryData inventoryData)
        {
            _blacksmithManager = blacksmithManager;
            _inventoryData = inventoryData;

            PanelMaterialsUpgrade[] panelsMaterials = _panelUpgradeMaterials.GetComponentsInChildren<PanelMaterialsUpgrade>();

            foreach (PanelMaterialsUpgrade panel in panelsMaterials)
            {
                _panelsMaterialsRequiriment.Add(panel);
                panel.gameObject.SetActive(false);
            }

            _textPlayerGold.text = _inventoryData.playerGold.ToString();
        }

        public void ShowRequiriment(EquipmentItem item, bool isMax)
        {
            _items = item;
            HideRequiriment();
            _hasSelected = true;

            _isMax = isMax;
            if (_isMax) return;

            int levelEquipment = item.equipmentLevel;
            _textPlayerGold.text = _inventoryData.playerGold.ToString();

            for (int i = 0; i < item.upgradeRequiriment[levelEquipment].materialRequiriment.Count; i++)
            {
                UpgradeMaterialRequiriment requiriment = item.upgradeRequiriment[levelEquipment].materialRequiriment[i];
                _panelsMaterialsRequiriment[i].Initialize(this, requiriment.itemRequiriment, GetMaterialsOwned(requiriment.itemRequiriment), requiriment.amount);
                _panelsMaterialsRequiriment[i].gameObject.SetActive(true);
            }

            SetUI(CheckRequiriment());
        }

        private void SetUI(bool isCanUpgrade)
        {
            if (isCanUpgrade)
            {
                _upgradePanelBackground.sprite = _canUpgradeBackground;
                _upgradePanelIcon.sprite = _canUpgradeIcon;
            }
            else
            {
                _upgradePanelBackground.sprite = _cannotUpgradeBackground;
                _upgradePanelIcon.sprite = _cannotUpgradeIcon;

            }
            _itemBeforeImage.sprite = _items.itemSprite;
            _itemAfterImage.sprite = _items.itemSprite;

            if (_items.specificType == ItemSlotType.Weapon)
            {
                _textStatBefore.text = _items.GetStatsWeapon(mainStat.AttackValue).ToString();
                _textStatAfter.text = _items.upgradeRequiriment[_items.equipmentLevel].GetStatsUpgradeWeapon(mainStat.AttackValue).ToString();

                _textLabelBefore.text = "Attack : ";
                _textLabelAfter.text = "Attack : ";
            }
            else if (_items.specificType == ItemSlotType.Shield)
            {
                _textStatBefore.text = _items.GetStatsWeapon(mainStat.Defense).ToString();
                _textStatAfter.text = _items.upgradeRequiriment[_items.equipmentLevel].GetStatsUpgradeWeapon(mainStat.Defense).ToString();

                _textLabelBefore.text = "Defense : ";
                _textLabelAfter.text = "Defense : ";
            }
            else if (_items.specificType == ItemSlotType.Armor)
            {
                _textStatBefore.text = _items.GetStatsWeapon(mainStat.Health).ToString();
                _textStatAfter.text = _items.upgradeRequiriment[_items.equipmentLevel].GetStatsUpgradeWeapon(mainStat.Health).ToString();

                _textLabelBefore.text = "Health Point : ";
                _textLabelAfter.text = "Health Point : ";
            }
        }

        private int GetMaterialsOwned(ABaseItem item)
        {
            int amount = 0;

            foreach (ItemSlotInfo items in _inventoryData.items)
            {
                if (items.item == item)
                {
                    amount += items.stacks;
                }
            }

            return amount;
        }

        private void ShowPanelConfirm()
        {
            AudioManager.instance.AudioPlayOneShot(AudioManager.instance.ConfirmUI, transform.position);

            if (!_hasSelected) return;

            if (CheckRequiriment())
            {
                _panelConfirm.SetActive(true);
                int price = _items.upgradeRequiriment[_items.equipmentLevel].price;
                _textPrice.text = price.ToString();

                if (price > _inventoryData.playerGold)
                {
                    _textPrice.color = Color.red;
                }
                else
                {
                    _textPrice.color = Color.white;
                }

                _hasSelectedUI = true;
            }
            else if (_isMax)
            {
                ShowPanelFailed("Equipment Sudah di Level Maksimal");
            }
            else
            {
                ShowPanelFailed("Materials Tidak Mencukupi");
            }
        }

        public void ShowPanelFailed(string textNotification)
        {
            _panelsFailedProces.SetActive(true);
            _textNotificationFailed.text = textNotification;
            _hasSelectedUI = true;
        }

        public void AttempToUpgrade()
        {
            if (_inventoryData.playerGold >= _items.upgradeRequiriment[_items.equipmentLevel].price)
            {
                Debug.Log("Success To Upgrade");
                SuccessToUpgrade();
                BackAction();
            }
            else
            {
                ShowPanelFailed("Gold Tidak Mencukupi");
            }
        }

        private void SuccessToUpgrade()
        {
            UseMaterialRequiriment();
            ReducePlayerMoney(_items.upgradeRequiriment[_items.equipmentLevel].price);
            UpgradeLevelEquipment();

            _blacksmithManager.ReShowPanel();
            _textPlayerGold.text = _inventoryData.playerGold.ToString();
        }

        private bool CheckRequiriment()
        {
            bool isAllready = true;

            foreach (PanelMaterialsUpgrade panel in _panelsMaterialsRequiriment)
            {
                if (panel.gameObject.activeSelf)
                {
                    if (!panel.isAllready)
                    {
                        return false;
                    }
                }
            }

            return isAllready;
        }

        private void HideRequiriment()
        {
            foreach (var panel in _panelsMaterialsRequiriment)
            {
                panel.gameObject.SetActive(false);
            }
        }

        public void BackAction()
        {
            if (_hasSelectedUI)
            {
                _panelConfirm.SetActive(false);
                _panelsFailedProces.SetActive(false);
                _hasSelectedUI = false;
            }
            else
            {
                AudioManager.instance.AudioPlayOneShot(AudioManager.instance.BackUI, transform.position);
                _blacksmithManager.CloseBlacksmith();
            }
        }

        private void UpgradeLevelEquipment()
        {
            _onUpgradeEquipment.RaiseEvent(_items);
        }

        private void UseMaterialRequiriment()
        {
            int equipmentLevel = _items.equipmentLevel;
            for (int i = 0; i < _items.upgradeRequiriment[equipmentLevel].materialRequiriment.Count; i++)
            {
                _onRemoveItemEvent.RaiseEvent(_items.upgradeRequiriment[equipmentLevel].materialRequiriment[i].itemRequiriment, _items.upgradeRequiriment[equipmentLevel].materialRequiriment[i].amount);
            }
        }

        private void ReducePlayerMoney(int amount)
        {
            _onUpdateCurrency.RaiseEvent(-amount);
        }
    }
}
