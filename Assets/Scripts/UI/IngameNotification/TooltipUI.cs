using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC
{
    public class TooltipUI : MonoBehaviour
    {
        TooltipItem target;
        string _text;
        [SerializeField] GameObject[] _visuals;
        [Header("Text Tooltip")]
        public string _shownText;
        public void ShowTooltip(Component sender, object data)
        {
            Reset();
            ToolTipData _data;
            if (data is ToolTipData) _data = (ToolTipData)data;
            else return;
            switch (_data.type)
            {
                case TooltipType.Text: _text = (string)data; ShowTextData(); break;
                case TooltipType.Item: ShowItemData(); break;
            }
        }

        void ShowTextData()
        {
            _visuals[0].SetActive(true);
            _shownText = _text;
        }

        void ShowItemData()
        {

        }

        void ShowEquipmentData()
        {

        }
        

        public void Reset()
        {
            target = null;
            _text = null;
            foreach(var vis in _visuals)
            {
                vis.SetActive(false);
            }
        }
    }
    public class ToolTipData
    {
        public TooltipType type;
        public object data;
        public ToolTipData(TooltipType type, object data)
        {
            this.type = type;
            this.data = data;
        }
    }

    public class TooltipItem : ABaseItem
    {
        ItemType type;
        public TooltipItem(ABaseItem data)
        {
            this.name = data.name;
            this.itemSprite = data.itemSprite;
            this.itemDescription = data.itemDescription;
            this.costSell = data.costSell;
            this.type = data.GetItemType();
            if(data.GetItemType() == ItemType.Equipment)
            {
                EquipmentItem e = (EquipmentItem)data;
            }
        }
        public override void UseItem()
        {
            return;
        }
        public override ItemType GetItemType()
        {
            return type;
        }
    }

    public enum TooltipType
    {
        Item,
        Equipment,
        Text,
    }
}
