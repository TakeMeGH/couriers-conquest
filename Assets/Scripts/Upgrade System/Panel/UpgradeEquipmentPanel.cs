using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CC.UpgradeEquipment
{
    public class UpgradeEquipmentPanel : AItemPanel
    {
        private bool click;
        private PanelUpgradeEquipmentAction _actionPanel;
        [SerializeField] private bool _isRequiriment;
        public override void OnEnable()
        {
            _actionPanel = new PanelUpgradeEquipmentAction();
            _actionPanel.SetRequiriment(_isRequiriment);
            _actionPanel.Initialize(this, inventory, mousePanel, itemSlot, itemImage, GetSlotType());
        }

        public void SetSpesifikRequiriment(UpgradeMaterialRequiriment item)
        {
            _actionPanel.SetSpesifikRequiriment(item);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            eventData.pointerPress = this.gameObject;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (mousePanel.itemSlot.item != null)
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
