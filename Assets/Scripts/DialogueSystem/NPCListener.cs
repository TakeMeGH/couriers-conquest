using CC.Event;
using CC.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.NPC
{
    public class NPCListener : MonoBehaviour
    {
        [SerializeField] NPCBehaviour _behaviour;
        [SerializeField] ComponentReturnEventChannelSO _event;
        public Component ListenQuest(Component sender, object data)
        {
            if (!_behaviour.isThisNPCCalled((string)data)) return null;
            return _behaviour;
        }
    }
}
