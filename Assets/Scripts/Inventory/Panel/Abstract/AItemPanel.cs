using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CC.Inventory
{
    public abstract class AItemPanel : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [SerializeField] private ItemSlotType _slotType;
        public IInventoryManager inventory;
        public ItemSlotMouse mousePanel;
        public ItemSlotInfo itemSlot;
        public Image itemImage;
        public TextMeshProUGUI stacksText;
        public GameObject equipedPanel;
        public bool isNull = true;

        public abstract void OnPointerClick(PointerEventData eventData);
        public abstract void OnPointerEnter(PointerEventData eventData);

        public ItemSlotType GetSlotType()
        {
            return _slotType;
        }

        public abstract void RefreshInventory();

        public virtual void ShowEquipedPanel(bool condition)
        {
            equipedPanel.SetActive(condition);
        }

        public abstract void OnAction();

        public abstract void Initialize(IInventoryManager inventory);
    }
}
