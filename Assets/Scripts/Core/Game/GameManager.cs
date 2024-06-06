using System.Collections;
using System.Collections.Generic;
using CC.Characters;
using CC.Core.Data.Dynamic;
using CC.Event;
using UnityEngine;
using Yarn.Unity;

namespace CC
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] SenderDataEventChannelSO _activateQuestEvent;
        [SerializeField] PlayerStateSO _playerStateData;
        [SerializeField] int _prologueQuestId;
        [SerializeField] InputReader _inputReader;
        [SerializeField] DialogueRunner _dialogueRunner;


        private void Start()
        {
            Time.timeScale = 1;
            if (!_playerStateData.IsFinishedQuest(_prologueQuestId))
            {
                _activateQuestEvent.raiseEvent(this, _prologueQuestId);
                if (_dialogueRunner == null)
                {
                    _dialogueRunner = FindObjectOfType<DialogueRunner>();
                    Invoke("OpenDialogue", 2f);
                    return;
                }
            }

            _inputReader.EnableGameplayInput();


        }
        public void OpenDialogue()
        {
            _inputReader.EnableInventoryUIInput();
            _dialogueRunner.StartDialogue("First_Dialogue");
        }

        // public void OnDialogueClosed()
        // {
        //     _inputReader.EnableGameplayInput();
        // }


    }
}
