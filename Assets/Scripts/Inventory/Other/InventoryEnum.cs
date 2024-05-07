using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    public enum ItemSlotType
    {
        Inventory,
        Weapon,
        Shield,
        Armor,
        Equipment,
        Consumable,
        Rune
    }

    public enum ItemType
    {
        Materials,
        Consumable,
        Rune,
        Equipment,
        QuestItem,
    }

    public enum ConsumableType
    {
        HPRegeneration,
        OnetimeATK,
        OvertimeATK,
        Stamina,
    }
}
