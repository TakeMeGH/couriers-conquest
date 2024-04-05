using CC.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Quest.Example
{
    public class ExPickupPoint : MonoBehaviour
    {
        [SerializeField] SenderDataEventChannelSO _onPickup;
        [SerializeField] QuestManager _manager;
        private void Start()
        {
            _manager = FindObjectOfType<QuestManager>();
        }
        public void doPickup()
        {
            _onPickup?.raiseEvent(this, null);
            Destroy(gameObject);
        }
    }
}
