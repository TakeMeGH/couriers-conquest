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
        _onInventoryOpen?.raiseEvent(this, null);
    }

    private void CloseInventory()
    {
        _onInventoryClose?.raiseEvent(this, null);
    }

}
