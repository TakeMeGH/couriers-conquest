using CC.Event;
using CC.Events;
using UnityEngine;

namespace CC.Inventory
{

    [CreateAssetMenu(fileName = "QuestItem", menuName = "Items/Quest", order = 5)]
    public class QuestItem : ABaseItem
    {
        [SerializeField] VoidEventChannelSO _onPlayerGetDamaged;
        [SerializeField] VoidEventChannelSO _onItemDestroyed;

        [Range(0f, 100f)] public float CurrentQuality;
        [Range(0f, 100f)] public float DefaultQuality;

        [SerializeField] float _valueReducePerDamage;
        [SerializeField] float _valueReducePerDrop;
        public override ItemType GetItemType()
        {
            return ItemType.QuestItem;
        }

        public override void UseItem()
        {
            Debug.Log("Use Item Quest " + itemName);
        }

        public void EnableDamageEvent()
        {
            _onPlayerGetDamaged.OnEventRaised += ReduceItemQualityOnDamaged;
        }

        public void DisableDamageEvent()
        {
            _onPlayerGetDamaged.OnEventRaised -= ReduceItemQualityOnDamaged;
        }

        public void ReduceItemQualityOnDrop()
        {
            CurrentQuality -= _valueReducePerDrop;
            CheckDestroyed();
        }

        public void ReduceItemQualityOnDamaged()
        {
            CurrentQuality -= _valueReducePerDamage;
            CheckDestroyed();
        }

        void CheckDestroyed()
        {
            if (CurrentQuality <= 0)
            {
                _onItemDestroyed.RaiseEvent();
            }
        }
    }
}
