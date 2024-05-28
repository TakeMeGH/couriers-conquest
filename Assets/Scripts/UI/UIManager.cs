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
    [SerializeField] GameObject _HUD;
    DialogueRunner _dialogueRunner;

    bool isInventoryOpen;


    private void OnEnable()
    {
        _inputReader.OpenInventoryEvent += OpenInventory;
        _inputReader.CloseInventoryEvent += CloseInventory;
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
        _dialogueRunner.onDialogueStart.RemoveListener(OnDialogueOpen);
        _dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueOpen);
    }


    public void OnDialogueOpen()
    {
        _HUD.SetActive(false);
    }

    public void OnDialogueComplete()
    {
        _HUD.SetActive(true);
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
