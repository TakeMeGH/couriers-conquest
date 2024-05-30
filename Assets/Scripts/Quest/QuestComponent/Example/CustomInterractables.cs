using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CC.Interaction
{
    public class CustomInterractables : MonoBehaviour, IInteraction
    {
        [SerializeField] Sprite _icon;
        [SerializeField] string _name;
        [SerializeField] int _amount = 1;
        [field:SerializeField] public bool IsAlwaysCanInteract { get; private set; } = true;
        public UnityEvent<Component> functionTrigger;
        public void Interact()
        {
            functionTrigger?.Invoke(this);
        }

        #region "Getter"
        public Sprite GetIcon() { return _icon; }
        public string GetName() { return _name; }
        public int GetAmount() { return _amount; }

        #endregion "Getter"

        #region "Setter"
        public void SetIcon(Sprite _icon) { this._icon = _icon; }
        public void SetName(string _name) { this._name = _name; }
        public void SetAmount(int _amount) { this._amount = _amount; }

        #endregion "Setter"

    }
}
