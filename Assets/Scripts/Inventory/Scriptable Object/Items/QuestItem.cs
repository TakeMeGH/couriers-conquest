using UnityEngine;

namespace CC.Inventory
{

    [CreateAssetMenu(fileName = "QuestItem", menuName = "Items/Quest", order = 5)]
    public class QuestItem : ABaseItem
    {
        public override ItemType GetItemType()
        {
            return ItemType.QuestItem;
        }

        public override void UseItem()
        {
            Debug.Log("Use Item Quest " + itemName);
        }
    }
}
