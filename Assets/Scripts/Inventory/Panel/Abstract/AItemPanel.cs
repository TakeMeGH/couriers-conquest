using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CC.Inventory
{
    public abstract class AItemPanel : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IDragHandler, IDropHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private ItemSlotType _slotType;
        public IInventoryManager inventory;
        public ItemSlotMouse mousePanel;
        public ItemSlotInfo itemSlot;
        public Image itemImage;
        public TextMeshProUGUI stacksText;

        public abstract void OnPointerClick(PointerEventData eventData);
        public abstract void OnPointerEnter(PointerEventData eventData);
        public abstract void OnBeginDrag(PointerEventData eventData);
        public abstract void OnEndDrag(PointerEventData eventData);
        public abstract void OnDrag(PointerEventData eventData);
        public abstract void OnDrop(PointerEventData eventData);

        public ItemSlotType GetSlotType()
        {
            return _slotType;
        }

        public abstract void RefreshInventory();

        public abstract void OnEnable();
    }
}
