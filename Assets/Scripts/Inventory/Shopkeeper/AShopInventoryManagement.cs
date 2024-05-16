using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    public abstract class AShopInventoryManagement : MonoBehaviour
    {
        protected IInventoryManager _inventoryManager;
        protected AInventoryData _inventoryData;
        protected ShopManager _shopManager;
        [SerializeField] protected AInventoryData _playerInventoryData;
        protected ItemSlotMouse _itemSlotMouse;

        [SerializeReference] protected List<AItemPanel> _inventoryShopPanel = new List<AItemPanel>();
        [SerializeField] protected GameObject _inventoryShopPanelUI;

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
    }
}
