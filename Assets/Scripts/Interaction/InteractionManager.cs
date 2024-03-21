using System.Collections.Generic;
using UnityEngine;

namespace CC.Interaction
{

    public class InteractionManager : MonoBehaviour
    {
        [SerializeField] private InputReader _inputReader = default;


        List<IInteraction> _potentialInteractions = new List<IInteraction>();

        private void OnEnable()
        {
            _inputReader.InteractPerformed += OnInteractionButtonPress;
        }

        private void OnDisable()
        {
            _inputReader.InteractPerformed -= OnInteractionButtonPress;
        }
        private void OnInteractionButtonPress()
        {
            if (_potentialInteractions.Count == 0)
                return;

            IInteraction currentInteraction = _potentialInteractions[0];
            currentInteraction.Interact();
        }

        public void OnTriggerChangeDetected(bool entered, GameObject obj)
        {
            if (entered)
                AddPotentialInteraction(obj);
            else
                RemovePotentialInteraction(obj);
        }

        private void AddPotentialInteraction(GameObject obj)
        {
            if (obj.TryGetComponent<IInteraction>(out IInteraction currentInteraction))
            {
                _potentialInteractions.Insert(0, currentInteraction); // insert first
            }
        }

        private void RemovePotentialInteraction(GameObject obj)
        {
            if (obj.TryGetComponent<IInteraction>(out IInteraction currentInteraction))
            {
                for (int i = 0; i < _potentialInteractions.Count; i++)
                {
                    if (_potentialInteractions[i] == currentInteraction)
                    {
                        _potentialInteractions.RemoveAt(i);
                        break;
                    }
                }
            }
        }

    }

}
