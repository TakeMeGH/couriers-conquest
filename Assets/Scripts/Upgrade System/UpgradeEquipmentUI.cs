using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;
using CC.Core.Data.Dynamic;


namespace CC.UpgradeEquipment
{
    public class UpgradeEquipmentUI : MonoBehaviour
    {
        private UpgradeEquipmentManager _upgradeManager;
        [SerializeField] private GameObject _panelSucces;
        [SerializeField] private GameObject _panelMaxLevel;


        [Space]
        [Header("Button Action")]
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private GameObject _shopCanvas;
        [SerializeField] private InputReader _inputReader;

        [Space]
        [Header("Text Component")]
        [SerializeField] private TextMeshProUGUI _textMoney;
        [SerializeField] private TextMeshProUGUI _textPrice;

        [Space]
        [Header("Panel Stats Before")]
        [SerializeField] private Image _imageBeforeEq;
        [SerializeField] private TextMeshProUGUI _textLevelBefore;
        [SerializeField] private TextMeshProUGUI _textAttackBefore;
        [SerializeField] private TextMeshProUGUI _textDefenseBefore;
        [SerializeField] private TextMeshProUGUI _textWeightBefore;

        [Space]
        [Header("Panel Stats Before")]
        [SerializeField] private Image _imageAfterEq;
        [SerializeField] private TextMeshProUGUI _textLevelAfter;
        [SerializeField] private TextMeshProUGUI _textAttackAfter;
        [SerializeField] private TextMeshProUGUI _textDefenseAfter;
        [SerializeField] private TextMeshProUGUI _textWeightAfter;

        public void Initialize(UpgradeEquipmentManager manager)
        {
            _upgradeManager = manager;
        }

        public void ShowUpgradeUI()
        {
            _shopCanvas.SetActive(true);
            _inputReader.EnableInventoryUIInput();
            Debug.Log("SHOW");
        }

        public void HideUpgradeUI()
        {
            _shopCanvas.SetActive(false);
            _inputReader.EnableGameplayInput();
            Debug.Log("HIDE");
        }

        public void SetButtonListener()
        {
            _upgradeButton.onClick.RemoveAllListeners();
            _upgradeButton.onClick.AddListener(_upgradeManager.UpgradeEquipment);
        }

        public void SetUpgradeButton(bool condition)
        {
            _upgradeButton.interactable = condition;
        }

        public void SetStatsPanelBeforeUpgrade(Sprite img, int level, SerializedDictionary<mainStat, float> EquipmentStats, float weight)
        {
            _imageBeforeEq.sprite = img;
            _textLevelBefore.text = "Equpment Level - " + level.ToString();

            float attackvalue = 0;
            EquipmentStats.TryGetValue(mainStat.AttackValue, out attackvalue);

            float deffenseValue = 0;
            EquipmentStats.TryGetValue(mainStat.Defense, out deffenseValue);

            _textAttackBefore.text = attackvalue.ToString();
            _textDefenseBefore.text = deffenseValue.ToString();
            _textWeightBefore.text = weight.ToString();
        }

        public void SetStatsPanelAfterUpgrade(Sprite img, int level, SerializedDictionary<mainStat, float> EquipmentStats, float weight)
        {
            _imageAfterEq.sprite = img;
            _textLevelAfter.text = "Equpment Level - " + level.ToString();

            float attackvalue = 0;
            EquipmentStats.TryGetValue(mainStat.AttackValue, out attackvalue);

            float deffenseValue = 0;
            EquipmentStats.TryGetValue(mainStat.Defense, out deffenseValue);

            _textAttackAfter.text = attackvalue.ToString();
            _textDefenseAfter.text = deffenseValue.ToString();
            _textWeightAfter.text = weight.ToString();

            _panelSucces.SetActive(true);
        }

        public void HidePanelSucces()
        {
            _panelSucces.SetActive(false);
        }

        public void UpdateTextPlayerMoney(float amount)
        {
            _textMoney.text = amount.ToString();
        }

        public void UpdateTextPrice(float amount)
        {
            _textPrice.text = amount.ToString();
        }

        public void SetMaxLevelLabelPanel(bool condition)
        {
            _panelMaxLevel.SetActive(condition);
        }
    }
}
