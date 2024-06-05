using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    public abstract class AInventoryManagement : MonoBehaviour
    {
        protected IInventoryManager _inventoryManager;
        protected AInventoryData _inventoryData;
        [SerializeField] protected AInventoryData _playerInventoryData;
        protected ItemSlotMouse _itemSlotMouse;

        [SerializeReference] protected List<AItemPanel> _inventoryItemPanel = new List<AItemPanel>();
        [SerializeField] protected GameObject _inventoryPanelUI;

        public virtual void RefreshedInventory(List<AItemPanel> itemPanel)
        {
            for (int i = 0; i < itemPanel.Count; i++)
            {
                //Name our List Elements
                itemPanel[i].name = "" + (i + 1);
                if (itemPanel[i].itemSlot.item != null) itemPanel[i].name += ": " + itemPanel[i].itemSlot.item.itemName;

                else itemPanel[i].name += ": -";

                //Update our Panels
                AItemPanel panel = itemPanel[i];
                panel.name = itemPanel[i].name + " Panel";
                if (panel != null)
                {
                    panel.inventory = _inventoryManager;
                    if (itemPanel[i].itemSlot.item != null)
                    {
                        panel.itemImage.gameObject.SetActive(true);
                        panel.itemImage.sprite = itemPanel[i].itemSlot.item.itemSprite;
                        panel.itemImage.CrossFadeAlpha(1, 0.05f, true);
                        panel.stacksText.gameObject.SetActive(true);

                        if (itemPanel[i].itemSlot.stacks > 1)
                        {
                            panel.stacksText.text = "" + itemPanel[i].itemSlot.stacks;
                        }
                        else
                        {
                            panel.stacksText.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        panel.itemImage.gameObject.SetActive(false);
                        panel.stacksText.gameObject.SetActive(false);
                    }
                }
            }
            _itemSlotMouse.EmptySlot();
        }

        public virtual void SetItemPanel(AItemPanel itemPanel, ItemSlotInfo slotInfo)
        {
            if (slotInfo != null)
            {
                itemPanel.itemSlot = new ItemSlotInfo(slotInfo.item, slotInfo.stacks);
                itemPanel.itemImage.sprite = slotInfo.item.itemSprite;
                itemPanel.itemImage.CrossFadeAlpha(1, 0.05f, true);
                itemPanel.name = slotInfo.item.itemName + " Panel";
                itemPanel.itemImage.gameObject.SetActive(true);
            }
            else
            {
                itemPanel.itemSlot = new ItemSlotInfo(null, 0);
            }

            itemPanel.mousePanel = _itemSlotMouse;
            itemPanel.inventory = _inventoryManager;
            itemPanel.Initialize(_inventoryManager);
        }

        public virtual void RefreshRequirimentPanel(AItemPanel itemPanel, UpgradeMaterialRequiriment item)
        {
            //Name our List Elements
            if (itemPanel.itemSlot.item != null) itemPanel.name += ": " + itemPanel.itemSlot.item.itemName;

            else itemPanel.name += ": -";

            //Update our Panels
            AItemPanel panel = itemPanel;
            panel.name = itemPanel.name + " Panel";
            if (panel != null)
            {
                panel.inventory = _inventoryManager;
                if (itemPanel.itemSlot.item != null)
                {
                    panel.itemImage.gameObject.SetActive(true);
                    panel.itemImage.sprite = itemPanel.itemSlot.item.itemSprite;
                    panel.itemImage.CrossFadeAlpha(1, 0.05f, true);
                    panel.stacksText.gameObject.SetActive(true);

                    if (itemPanel.itemSlot.stacks > 1)
                    {
                        panel.stacksText.text = "" + itemPanel.itemSlot.stacks;
                    }
                    else
                    {
                        panel.stacksText.gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (item.itemRequiriment)
                    {
                        panel.itemImage.gameObject.SetActive(true);
                        panel.itemImage.sprite = item.itemRequiriment.itemSprite;
                        panel.stacksText.text = item.amount.ToString();
                        panel.itemImage.CrossFadeAlpha(0.3f, 0.05f, true);
                        panel.stacksText.gameObject.SetActive(true);
                    }
                    else
                    {
                        panel.itemImage.gameObject.SetActive(false);
                        panel.stacksText.gameObject.SetActive(false);
                    }
                }
            }
            _itemSlotMouse.EmptySlot();
        }

        public virtual void RefreshEquipmentSlotPanel(AItemPanel itemPanel)
        {
            AItemPanel panel = itemPanel;
            panel.name = itemPanel.name + " Panel";
            if (panel != null)
            {
                panel.inventory = _inventoryManager;
                if (itemPanel.itemSlot.item != null)
                {
                    panel.itemImage.gameObject.SetActive(true);
                    panel.itemImage.sprite = itemPanel.itemSlot.item.itemSprite;
                    panel.itemImage.CrossFadeAlpha(1, 0.05f, true);
                    panel.stacksText.gameObject.SetActive(true);

                    if (itemPanel.itemSlot.stacks > 1)
                    {
                        panel.stacksText.text = "" + itemPanel.itemSlot.stacks;
                    }
                    else
                    {
                        panel.stacksText.gameObject.SetActive(false);
                    }
                }
                else
                {
                    panel.itemImage.gameObject.SetActive(false);
                    panel.stacksText.gameObject.SetActive(false);
                }
            }

            _itemSlotMouse.EmptySlot();
        }

        /*public virtual void RefreshInventoryBaseOnInventory(List<AItemPanel> itemPanel)
        {
            int index = 0;
            foreach (ItemSlotInfo i in _inventoryData.items)
            {
                //Name our List Elements
                i.name = "" + (index + 1);
                if (i.item != null) i.name += ": " + i.item.itemName;
                else i.name += ": -";

                //Update our Panels
                AItemPanel panel = itemPanel[index];
                panel.name = i.name + " Panel";
                if (panel != null)
                {
                    panel.inventory = _inventoryManager;
                    panel.itemSlot = i;
                    if (i.item != null)
                    {
                        panel.itemImage.gameObject.SetActive(true);
                        panel.itemImage.sprite = i.item.itemSprite;
                        panel.itemImage.CrossFadeAlpha(1, 0.05f, true);
                        panel.stacksText.gameObject.SetActive(true);

                        if (i.stacks > 1)
                        {
                            panel.stacksText.text = "" + i.stacks;
                        }
                        else
                        {
                            panel.stacksText.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        panel.itemImage.gameObject.SetActive(false);
                        panel.stacksText.gameObject.SetActive(false);
                    }
                }
                index++;
            }
            _itemSlotMouse.EmptySlot();
        }*/

        public int OnAddItem(ABaseItem item, int amount, List<AItemPanel> itemPanel)
        {
            Debug.Log("Try To add Item");
            //Check for open spaces in existing slots
            foreach (AItemPanel i in itemPanel)
            {
                if (i.itemSlot.item != null)
                {
                    if (i.itemSlot.item == item)
                    {
                        if (amount > i.itemSlot.item.maxStacks - i.itemSlot.stacks)
                        {
                            amount -= i.itemSlot.item.maxStacks - i.itemSlot.stacks;
                            i.itemSlot.stacks = i.itemSlot.item.maxStacks;
                        }
                        else
                        {
                            i.itemSlot.stacks += amount;
                            return 0;
                        }
                    }
                }
            }

            //Fill empty slots with leftover items
            foreach (AItemPanel i in itemPanel)
            {
                if (i.itemSlot.item == null)
                {
                    if (amount > item.maxStacks)
                    {
                        i.itemSlot.item = item;
                        i.itemSlot.stacks = item.maxStacks;
                        amount -= item.maxStacks;
                    }
                    else
                    {
                        i.itemSlot.item = item;
                        i.itemSlot.stacks = amount;
                        return 0;
                    }
                }
            }
            return amount;
        }
    }
}
