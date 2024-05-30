using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CC.Inventory
{
    public class PanelInventory : AItemPanel
    {
        private bool click;
        private IPanelAction _actionPanel;
        private PlayerInventoryManager _playerInventoryManager;
        public int itemIndex;
        [SerializeField] private Image _frameItem;

        public override void Initialize(IInventoryManager inventoryManager)
        {
            inventory = inventoryManager;
            _actionPanel = new PanelInventoryAction();
            _actionPanel.Initialize(this, inventory, mousePanel, itemSlot, itemImage, GetSlotType());
            _playerInventoryManager = (PlayerInventoryManager)inventory;
        }

        public void OnEnable()
        {
            _actionPanel.Initialize(this, inventory, mousePanel, itemSlot, itemImage, GetSlotType());
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            eventData.pointerPress = this.gameObject;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (isNull) return;

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (itemSlot.item == null) return;

                //Debug.Log("OnAction");

                _playerInventoryManager.SwapActiveSlot(itemIndex);
                _actionPanel.OnAction();
                _playerInventoryManager.itemDetailPanel.SetActive(true);
            }
        }

        public override void ShowEquipedPanel(bool condition)
        {
            base.ShowEquipedPanel(condition);
        }

        public override void RefreshInventory()
        {
            _actionPanel.RefreshInventory();
        }

        public override void OnAction()
        {
            _actionPanel.OnAction();
        }

        public void ChangeFrameSlotUI(Sprite frame)
        {
            if (itemSlot.item == null) return;
            if (itemSlot.item.GetItemType() == ItemType.Equipment) return;    

            _frameItem.sprite = frame;
        }

        public void ForceChangeFrame(Sprite frame)
        {
            _frameItem.sprite = frame;
        }
    }
}
