using System;
using UnityEngine;

/// <summary>
/// This class is used for Events that have no arguments (Example: Exit game event)
/// </summary>

namespace CC.Events
{
    [CreateAssetMenu(menuName = "Game/Events/Interact Event UI")]
    public class InteractEventUISO : DescriptionBaseSO
    {
        public Func<Sprite, int, string, int> OnEventRaised;

        public int RaiseEvent(Sprite _icon, int _amount, string _interactionName)
        {
            if (OnEventRaised != null)
                return OnEventRaised.Invoke(_icon, _amount, _interactionName);
            else
                return -1;
        }
    }

}


