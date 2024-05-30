using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CC
{
    public class PanelInventoryBuy : AItemPanel
    {
        private bool click;
        private IPanelAction _actionPanel;

        public override void Initialize(IInventoryManager inventoryManager)
        {
            inventory = inventoryManager;
            _actionPanel = new PanelInventoryAction();
            _actionPanel.Initialize(this, inventory, mousePanel, itemSlot, itemImage, GetSlotType());
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            eventData.pointerPress = this.gameObject;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (mousePanel.itemSlot.item == null)
                {
                    if (itemSlot.item != null)
                    {
                        itemSlot.item.UseItem();
                    }
                }
                else if (mousePanel.itemSlot.item != null)
                {
                    _actionPanel.OnAction();
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (mousePanel.itemSlot.item == null)
                {
                    Debug.Log("removed");
                    RemoveSelected();
                    mousePanel.EmptySlot();
                    RefreshInventory();
                }
            }
        }

        public override void RefreshInventory()
        {
            _actionPanel.RefreshInventory();
        }

        private void RemoveSelected()
        {
            itemSlot.item = null;
            itemSlot.stacks = 0;
            itemImage.gameObject.SetActive(false);
            stacksText.gameObject.SetActive(false);
        }

        public override void OnAction()
        {
            throw new System.NotImplementedException();
        }
    }
}
