using System;
using UnityEngine;

namespace CC.Events
{
    [CreateAssetMenu(menuName = ("Game/Events/ReturnObjectEventChannel"))]
    public class ObjectReturnEventChannelSO : MonoBehaviour
    {
        public Func<Component, object, object> OnEventRaised;
        public object raiseEvent(Component sender, object data)
        {
            return OnEventRaised?.Invoke(sender, data);
        }
    }
}
