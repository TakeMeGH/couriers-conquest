using CC.Events;
using CC.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC
{
    public class SellShopkeeperInteract : MonoBehaviour, IInteraction
    {
        [SerializeField] private VoidEventChannelSO onTriggerSellEvent;

        public void Interact()
        {
            ShowSellfeature();
        }

        private void ShowSellfeature()
        {
            onTriggerSellEvent.RaiseEvent();
        }
    }
}
