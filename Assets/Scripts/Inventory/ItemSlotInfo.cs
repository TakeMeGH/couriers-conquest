

namespace cc_inventory {

    [System.Serializable]
    public class ItemSlotInfo
    {
        public ABaseItem item;
        public string name;
        public int stacks;

        public ItemSlotInfo(ABaseItem newItem, int newStacks)
        {
            item = newItem;
            stacks = newStacks;
        }
    }
}
