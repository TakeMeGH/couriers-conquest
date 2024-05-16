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

        public override void OnEnable()
        {
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
                if (mousePanel.itemSlot.item != null)
                {
                    inventory.RefreshInventory();
                }
            }
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            click = true;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (click)
            {
                _actionPanel.OnAction();
                click = false;
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (click)
            {
                _actionPanel.OnAction();
                click = false;
            }
        }
        public override void OnDrop(PointerEventData eventData)
        {
            _actionPanel.OnAction();
            click = false;
        }

        public override void RefreshInventory()
        {
            _actionPanel.RefreshInventory();
        }
    }
}
