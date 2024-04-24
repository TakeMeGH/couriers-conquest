using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have no arguments (Example: Exit game event)
/// </summary>

namespace CC.Events
{
    [CreateAssetMenu(menuName = "Game/Events/Int Event UI")]
    public class IntEventChannelSO : DescriptionBaseSO
    {
        public UnityAction<int> OnEventRaised;

        public void RaiseEvent(int value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
        }
    }

}


