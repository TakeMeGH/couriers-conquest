using CC.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace CC.NPC
{
    public class NPCBehaviour : MonoBehaviour
    {
        [Header("Identifier")]
        [SerializeField] string NPC_id;
        [SerializeField] string NPC_name;
        [Header("Node")]
        public string Default_dialogue_node;
        public string Quest_dialogue_node;
        [Header("Event")]
        public SenderDataEventChannelSO npcAcceptedQuestEvent;
        public SenderDataEventChannelSO[] customEvents;
        [Header("Dependency")]
        [SerializeField] InputReader _inputReader;
        [SerializeField] DialogueRunner _dialogueRunner;
        [SerializeField] VariableStorageBehaviour _dialogueVariable;
        private void Start()
        {
            if (_dialogueRunner == null) FindObjectOfType<DialogueRunner>();
            _dialogueRunner.AddCommandHandler("Close",() => OnDialogueClosed());

        }

        public void ListenQuest(Component sender, object data)
        {
            if(data is NPCQuestDialogueNodeTransfer)
            {
                NPCQuestDialogueNodeTransfer _data = (NPCQuestDialogueNodeTransfer)data;
                if(NPC_id == _data.targetNPC)
                {
                    Quest_dialogue_node = _data.targetNPC;
                    npcAcceptedQuestEvent?.raiseEvent(this, null);
                }
            }
        }

        public void OpenDialogue()
        {
            Cursor.lockState = CursorLockMode.Confined;
            _inputReader.EnableInventoryUIInput();
            _dialogueVariable.SetValue("$HaveQuest", Quest_dialogue_node != "");
            _dialogueVariable.SetValue("$QuestNode",Quest_dialogue_node);
            _dialogueRunner.StartDialogue(Default_dialogue_node);
        }

        public void emitEvent(int index)
        {
            customEvents[index]?.raiseEvent(this, null);
        }

        public void OnDialogueClosed()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _inputReader.EnableGameplayInput();
        }

        public void clearQuestDialogue()
        {
            Quest_dialogue_node = "";
        }
    }
    public struct NPCQuestDialogueNodeTransfer
    {
        public string targetNPC;
        public string dialogueNode;
    }
}
