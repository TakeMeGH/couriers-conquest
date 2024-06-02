using System.Collections;
using System.Collections.Generic;
using CC.Event;
using CC.Quest.UI;
using UnityEngine;

namespace CC
{
    public class GameObjectIdentifier : MonoBehaviour
    {
        public int ID;
        [SerializeField] SenderDataEventChannelSO _questHint;

        public void OnGameObjectListen(Component sender, object data)
        {
            if (data is QuestHintData)
            {
                QuestHintData temp = (QuestHintData)data;
                Debug.Log(ID + " ID " + temp.ID);

                if (ID != temp.ID) return;
                _questHint?.raiseEvent(null, new HintData(temp.questObjective, gameObject));
            }
        }
    }
    public class HintData
    {
        public readonly string Objective;
        public readonly GameObject Destination;
        public HintData(string objective, GameObject destination)
        {
            this.Objective = objective;
            this.Destination = destination;
        }
    }
}
