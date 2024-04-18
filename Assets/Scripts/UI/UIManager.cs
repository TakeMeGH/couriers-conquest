using System.Collections;
using System.Collections.Generic;
using CC.Event;
using CC.Events;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] InputReader _inputReader;
    [SerializeField] SenderDataEventChannelSO _onInventoryOpen;
    [SerializeField] SenderDataEventChannelSO _onInventoryClose;
    bool isInventoryOpen;


    private void OnEnable()
    {
        _inputReader.OpenInventoryEvent += OpenInventory;
        _inputReader.CloseInventoryEvent += CloseInventory;
    }

    private void OnDisable()
    {
        _inputReader.OpenInventoryEvent -= OpenInventory;
        _inputReader.CloseInventoryEvent -= CloseInventory;

    }

    private void OpenInventory()
    {
        if(isInventoryOpen) return;
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
