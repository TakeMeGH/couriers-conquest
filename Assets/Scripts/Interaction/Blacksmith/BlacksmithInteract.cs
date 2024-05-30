using CC.Events;
using CC.Inventory;
using CC.Shop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Interaction.Blacksmith
{
    public class BlacksmithInteract : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _onUpgradeEvent;

        public void Interact()
        {
            ShowBlackSmith();
        }

        public void ShowBlackSmith()
        {
            _onUpgradeEvent.RaiseEvent();
        }
    }
}
