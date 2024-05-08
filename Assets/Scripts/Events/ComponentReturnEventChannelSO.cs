using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Event
{
    [CreateAssetMenu(menuName = ("Game/Events/ReturnObjectEventChannel"))]
    public class ComponentReturnEventChannelSO : MonoBehaviour
    {
        public Func<Component, object, Component> OnEventRaised;
        public Component raiseEvent(Component sender, object data)
        {
            return OnEventRaised?.Invoke(sender, data);
        }
    }
}
