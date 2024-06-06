using System.Collections;
using System.Collections.Generic;
using CC.Event;
using CC.Events;
using UnityEngine;
using Yarn.Unity;

public class UIManager : MonoBehaviour
{
    [SerializeField] InputReader _inputReader;
    [SerializeField] SenderDataEventChannelSO _onInventoryOpen;
    [SerializeField] SenderDataEventChannelSO _onInventoryClose;
    [SerializeField] VoidEventChannelSO _onCharacterDamaged;
    [SerializeField] CanvasGroup _HUD;
    DialogueRunner _dialogueRunner;

    bool isInventoryOpen;


    private void OnEnable()
    {
        _inputReader.OpenInventoryEvent += OpenInventory;
        _inputReader.CloseInventoryEvent += CloseInventory;
        _onCharacterDamaged.OnEventRaised += CloseInventory;
        if (_dialogueRunner != null)
        {
            _dialogueRunner.onDialogueStart.AddListener(OnDialogueOpen);
            _dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);

        }

    }

    private void Start()
    {
        if (_dialogueRunner == null) _dialogueRunner = FindObjectOfType<DialogueRunner>();

        _dialogueRunner.onDialogueStart.AddListener(OnDialogueOpen);
        _dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);

    }

    private void OnDisable()
    {
        _inputReader.OpenInventoryEvent -= OpenInventory;
        _inputReader.CloseInventoryEvent -= CloseInventory;
        _onCharacterDamaged.OnEventRaised -= CloseInventory;

        _dialogueRunner.onDialogueStart.RemoveListener(OnDialogueOpen);
        _dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueOpen);
    }


    public void OnDialogueOpen()
    {
        _HUD.alpha = 0;
    }

    public void OnDialogueComplete()
    {
        _HUD.alpha = 1;
    }

    private void OpenInventory()
    {
        if (isInventoryOpen) return;
        isInventoryOpen = true;
        _onInventoryOpen?.raiseEvent(this, null);
    }

    private void CloseInventory()
    {
        if (!isInventoryOpen) return;
        isInventoryOpen = false;
        _onInventoryClose?.raiseEvent(this, null);
    }

}
