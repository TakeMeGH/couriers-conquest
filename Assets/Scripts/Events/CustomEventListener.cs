using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CC.Event
{
    public class CustomEventListener : MonoBehaviour
    {
        public List<CustomConnector> listeners;
        private void OnEnable()
        {
            foreach (var connector in listeners) { connector?.assign(); }
        }
        private void OnDisable()
        {
            foreach(var connector in listeners) { connector?.clear(); }
        }
    }
    [System.Serializable]
    public class CustomConnector
    {
        public SenderDataEventChannelSO channel;
        public customEvent eventToCall;
        public void assign()
        {
            channel.OnEventRaised.AddListener(call);
        }
        public void call(Component sender, object data)
        {
            Debug.Log("Calling event");
            eventToCall?.Invoke(sender, data);
        }
        public void clear()
        {
            channel.OnEventRaised.RemoveListener(call);
        }
    }

    [System.Serializable]
    public class customEvent : UnityEvent<Component, object> { }
}
