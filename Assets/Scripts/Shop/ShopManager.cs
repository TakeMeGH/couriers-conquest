using CC.Events;
using CC.Shop;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace CC.Inventory
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] InputReader _inputReader;
        [SerializeField] private InventoryData _payerInventoryData;

        [SerializeField] private ItemSlotMouse _itemSlotMouseBuy;
        [SerializeField] private ItemSlotMouse _itemSlotMouseSell;
        [SerializeField] private BuyManager _buyManager;
        [SerializeField] private SellManager _sellManager;

        [Space]
        [Header("UI Component")]
        [SerializeField] private GameObject[] _listOtherPanel;
        [SerializeField] private Volume _blurEffect;

        [Header("Event Panel")]
        [SerializeField] private NPCInventoryDataChannel _onEventBuy;
        [SerializeField] private OnUpdateCurrencyEventChannel _onUpdateCurrency;
        [SerializeField] private VoidEventChannelSO _onEventSell;

        private void OnEnable()
        {
            _onEventBuy.OnEventRaised += InitializeBuyEvent;
            _onEventSell.OnEventRaised += InitializeSellEvent;
            _sellManager.InitComponent();
            _buyManager.InitComponent();
        }

        private void OnDisable()
        {
            _onEventBuy.OnEventRaised -= InitializeBuyEvent;
            _onEventSell.OnEventRaised -= InitializeSellEvent;
        }

        private void InitializeBuyEvent(ItemShopkeeper shopkeeperInventoryData)
        {
            _inputReader.EnableInventoryUIInput();
            _buyManager.Initialize(_payerInventoryData, this, _itemSlotMouseBuy, shopkeeperInventoryData);

            _buyManager.gameObject.SetActive(true);
            HideOtherUI();
        }

        private void InitializeSellEvent()
        {
            _inputReader.EnableInventoryUIInput();
            _sellManager.Initialize(_payerInventoryData, this, _itemSlotMouseSell);

            _sellManager.gameObject.SetActive(true);
            HideOtherUI();
        }

        public void ShowOtherUI()
        {
            foreach (GameObject panel in _listOtherPanel)
            {
                panel.SetActive(true);
            }

            _blurEffect.enabled = false;
        }

        public void HideOtherUI()
        {
            foreach (GameObject panel in _listOtherPanel)
            {
                panel.SetActive(false);
            }

            _blurEffect.enabled = true;
        }
    }
}
