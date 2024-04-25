using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CC.Event
{
    [CreateAssetMenu(menuName =("Game/Events/SenderDataEventChannel"))]
    public class SenderDataEventChannelSO : DescriptionBaseSO
    {
        public UnityEvent<Component, object> OnEventRaised;
        public void raiseEvent(Component sender, object data)
        {
            OnEventRaised?.Invoke(sender, data);
        }
    }
}
