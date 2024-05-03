using CC.Event;
using CC.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

namespace CC.NPC
{
    public class NPCBehaviour : MonoBehaviour
    {
        [Header("Identifier")]
        [SerializeField] string _NPCID;
        [SerializeField] string _NPCName;
        [Header("Node")]
        public string Default_dialogue_node;
        public string Quest_dialogue_node;
        [Header("Event")]
        public SenderDataEventChannelSO _npcAcceptedQuestEvent;
        public List<SenderDataEventChannelSO> _customEvents;
        public UnityAction[] _specialAction;
        [Header("Dependency")]
        [SerializeField] InputReader _inputReader;
        [SerializeField] DialogueRunner _dialogueRunner;
        [SerializeField] VariableStorageBehaviour _dialogueVariable;
        private void Start()
        {
            if (_dialogueRunner == null) FindObjectOfType<DialogueRunner>();
            _dialogueRunner.AddCommandHandler("Close",() => OnDialogueClosed());
            //_dialogueRunner.AddCommandHandler("SpecialAction", (int index) => SpecialAction(index));
            //_dialogueRunner.AddCommandHandler("CustomAction", (int index) => emitEvent(index));
        }

        public void OpenDialogue()
        {
            Cursor.lockState = CursorLockMode.Confined;
            _inputReader.EnableInventoryUIInput();
            _dialogueVariable.SetValue("$" + _NPCName + "HaveQuest", Quest_dialogue_node != "");
            _dialogueVariable.SetValue("$" + _NPCName + "QuestNode",Quest_dialogue_node);
            _dialogueRunner.StartDialogue(Default_dialogue_node);
        }

        public void OnQuestListen(Component sender, object data)
        {
            //if (!(sender.GetType() == typeof(AQuest))) return;
            if (data is NPCQuestDialogueNodeTransfer)
            {
                NPCQuestDialogueNodeTransfer temp = (NPCQuestDialogueNodeTransfer)data;
                if (!isThisNPCCalled(temp.targetNPC)) return;
                Debug.Log("npc " + _NPCName + " is listening quest");
                Quest_dialogue_node = temp.dialogueNode;
                foreach (var e in temp.events) _customEvents.Add(e);
                _npcAcceptedQuestEvent?.raiseEvent(this, null);
            }
        }

        [YarnCommand("CustomAction")]
        public void emitEvent(int index)
        {
            _customEvents[index]?.raiseEvent(this, null);
        }

        public void OnDialogueClosed()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _inputReader.EnableGameplayInput();
        }

        public void clearQuestDialogue()
        {
            Quest_dialogue_node = "";
            _customEvents.Clear();
        }

        public bool isThisNPCCalled(string id)
        {
            return id == _NPCID;
        }
        [YarnCommand("SpecialAction")]
        public void SpecialAction(int index)
        {
            _specialAction[index].Invoke();
        }
    }
    [System.Serializable]
    public struct NPCQuestDialogueNodeTransfer
    {
        public string targetNPC;
        public string dialogueNode;
        public SenderDataEventChannelSO[] events;
    }
}
