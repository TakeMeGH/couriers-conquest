using CC.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Quest
{
    public class NPCQuestEx : MonoBehaviour
    {
        [SerializeField] SenderDataEventChannelSO _onInteract;
        [SerializeField] InputReader _inputReader;
        public void showQuest()
        {
            Cursor.lockState = CursorLockMode.Confined;
            _inputReader.EnableInventoryUIInput();
            _onInteract?.raiseEvent(this, null);
        }
    }
}
