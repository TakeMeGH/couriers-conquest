using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace CC.Inventory
{
    public class PanelEquipment : AItemPanel
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
                _actionPanel.OnAction();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (mousePanel.itemSlot.item != null)
                {
                    inventory.RefreshInventory();
                }
            }
        }


        public override void RefreshInventory()
        {
            _actionPanel.RefreshInventory();
        }
    }
}
