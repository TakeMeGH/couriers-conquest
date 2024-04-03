using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CC.Interaction
{
    public class CustomInterractables : MonoBehaviour,IInteraction
    {
        [SerializeField] Sprite _icon;
        [SerializeField] string _name;
        public UnityEvent<Component> functionTrigger;
        public void Interact()
        {
            functionTrigger?.Invoke(this);
        }

        #region "Getter"
        public Sprite GetIcon() { return _icon; }
        public string GetName() { return _name; }
        #endregion "Getter"
    }
}
