using System.Collections.Generic;
using CC.Inventory;
using CC.Items;
using UnityEngine;

namespace CC
{
    [CreateAssetMenu(fileName = "Enemy Drop Item Config SO", menuName = "Game/Enemy Drop Item Config")]
    public class EnemyDropItemSO : ScriptableObject
    {
        [SerializeField] ListItemID _listItemID;
        [SerializeField] List<DropItem> _possibleDropItem;
        [field: SerializeField] public int Exp { get; private set; }

        public List<DropedItem> GetDroppedItems()
        {
            List<DropedItem> droppedItems = new();

            foreach (var item in _possibleDropItem)
            {
                if (Random.Range(0f, 1f) <= item.Chance)
                {
                    int amountToDrop = GetRandomAmount(item.Amount);
                    ABaseItem itemData = _listItemID.GetItemByID(item.ItemID);
                    droppedItems.Add(new DropedItem { Item = itemData, Amount = amountToDrop });
                }
            }
            return droppedItems;
        }

        private int GetRandomAmount(int maxAmount)
        {
            List<int> weights = new List<int>();
            for (int i = 1; i <= maxAmount; i++)
            {
                weights.Add(maxAmount - i + 1);
            }

            int totalWeight = 0;
            foreach (int weight in weights)
            {
                totalWeight += weight;
            }

            int randomWeight = Random.Range(0, totalWeight);

            int cumulativeWeight = 0;
            for (int i = 0; i < weights.Count; i++)
            {
                cumulativeWeight += weights[i];
                if (randomWeight < cumulativeWeight)
                {
                    return i + 1;
                }
            }

            return 1;
        }
    }

    [System.Serializable]
    public class DropItem
    {
        [Range(0, 1)]
        public float Chance;
        public int Amount;
        public string ItemID;
    }

    public class DropedItem
    {
        public ABaseItem Item;
        public int Amount;
    }


}
