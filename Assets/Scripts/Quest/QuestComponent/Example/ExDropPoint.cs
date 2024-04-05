using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CC.Event;

namespace CC.Quest.Example
{
    public class ExDropPoint : MonoBehaviour
    {
        [SerializeField] SenderDataEventChannelSO _onDrop;
        [SerializeField] QuestManager _manager;
        private void Start()
        {
            _manager = FindObjectOfType<QuestManager>();
        }
        public void doDrop()
        {
            var temp = _manager.GetAcceptedQuest();
            if (temp is not ExampleQuestSO) return;
            if (((ExampleQuestSO)temp).IsPickedUp()) _onDrop?.raiseEvent(this, null);
        }
    }
}
