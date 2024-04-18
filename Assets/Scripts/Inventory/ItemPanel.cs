using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace CC.Inventory
{
    public class ItemPanel : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IDragHandler, IDropHandler, IBeginDragHandler, IEndDragHandler
    {
        public InventoryManager inventory;
        public Mouse mouse;
        public ItemSlotInfo itemSlot;
        public Image itemImage;
        public TextMeshProUGUI stacksText;
        public ItemSlotType slotType;

        private bool click;

        public void OnPointerEnter(PointerEventData eventData)
        {
            eventData.pointerPress = this.gameObject;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (CheckSlotType()) return;

            click = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (click)
            {
                OnAction();
                click = false;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (mouse.itemSlot.item == null)
                {
                    if(itemSlot.item != null)
                    {
                        itemSlot.item.UseItem();
                    }
                }
                else if (mouse.itemSlot.item != null)
                {
                    OnAction();
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (mouse.itemSlot.item != null)
                {
                    inventory.RefreshInventory();
                }
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (CheckSlotType()) return;

            OnAction();
            click = false;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (CheckSlotType()) return;

            if (click)
            {
                OnAction();
                click = false;
            }
        }

        private bool CheckSlotType()
        {
            return slotType == ItemSlotType.Inventory || slotType == ItemSlotType.Consumable ? false : true;
        }

        private void PickupItem()
        {
            mouse.itemSlot = itemSlot;
            mouse.sourceItemPanel = this;
            if (Input.GetKey(KeyCode.LeftShift) && itemSlot.stacks > 1) mouse.splitSize = itemSlot.stacks / 2;
            else mouse.splitSize = itemSlot.stacks;
            mouse.SetUI();

        }
        private void FadeOut()
        {
            itemImage.CrossFadeAlpha(0.3f, 0.05f, true);
        }

        private void DropItem()
        {
            itemSlot.item = mouse.itemSlot.item;
            if (mouse.splitSize < mouse.itemSlot.stacks)
            {
                itemSlot.stacks = mouse.splitSize;
                mouse.itemSlot.stacks -= mouse.splitSize;
                mouse.EmptySlot();
            }
            else
            {
                itemSlot.stacks = mouse.itemSlot.stacks;
                inventory.ClearSlot(mouse.itemSlot);
            }
        }
        private void SwapItem(ItemSlotInfo slotA, ItemSlotInfo slotB)
        {
            ItemSlotInfo tempItem = new ItemSlotInfo(slotA.item, slotA.stacks);

            slotA.item = slotB.item;
            slotA.stacks = slotB.stacks;

            slotB.item = tempItem.item;
            slotB.stacks = tempItem.stacks;
        }
        private void StackItem(ItemSlotInfo source, ItemSlotInfo destination, int amount)
        {
            int slotsAvailable = destination.item.maxStacks - destination.stacks;
            if (slotsAvailable == 0) return;

            if (amount > slotsAvailable)
            {
                source.stacks -= slotsAvailable;
                destination.stacks = destination.item.maxStacks;
            }
            if (amount <= slotsAvailable)
            {
                destination.stacks += amount;
                if (source.stacks == amount) inventory.ClearSlot(source);
                else source.stacks -= amount;
            }
        }

        private void OnAction()
        {
            if (inventory != null)
            {
                mouse = inventory.mouse;

                //Grab item if mouse slot is empty
                if (mouse.itemSlot.item == null)
                {
                    if (itemSlot.item != null)
                    {
                        PickupItem();
                        FadeOut();
                    }
                }

                else
                {
                    if(slotType == ItemSlotType.Inventory)
                    {
                        OnActionInventory();
                    }
                    else if(slotType == ItemSlotType.Consumable)
                    {
                        OnActionConsumableItem();
                    }
                    else 
                    {
                        OnActionEquipmentSlot();
                    }
                }
            }
        }

        private void OnActionInventory()
        {
            //Clicked on original slot
            if (itemSlot == mouse.itemSlot)
            {
                mouse.EmptySlot();
            }
            //Clicked on empty slot
            else if (itemSlot.item == null)
            {
                DropItem();
            }
            else if (itemSlot.item != mouse.itemSlot.item)
            {
                SwapItem(itemSlot, mouse.itemSlot);
            }
            //Clicked on occupied slot of different item type
            else if (itemSlot.item.maxStacks != mouse.itemSlot.item.maxStacks)
            {
                SwapItem(itemSlot, mouse.itemSlot);
            }
            //Clicked on occupided slot of same type
            else if (itemSlot.stacks < itemSlot.item.maxStacks)
            {
                StackItem(mouse.itemSlot, itemSlot, mouse.splitSize);
            }
            else if (itemSlot.stacks >= itemSlot.item.maxStacks)
            {
                SwapItem(itemSlot, mouse.itemSlot);
            }
            else
            {
                mouse.EmptySlot();
            }

            inventory.RefreshInventory();
        }

        private void OnActionEquipmentSlot()
        {
            if (mouse.itemSlot.item.GetItemType() != ItemType.Equipment) {
                Debug.Log("Slot Not Match");
                return;
            };

            EquipmentItem equipmentScript = (EquipmentItem)mouse.itemSlot.item;

            if (slotType == equipmentScript.specificType)
            {
                OnActionInventory();
            }
            else
            {
                Debug.Log("Slot Not Match");
            }
        }

        private void OnActionConsumableItem()
        {
            if(mouse.itemSlot.item.GetItemType() == ItemType.Consumable)
            {
                OnActionInventory();
            }
            else
            {
                Debug.Log("Slot Not Match");
            }
        }

        public void RefreshInventory()
        {
            inventory.RefreshInventory();
        }
    }
}