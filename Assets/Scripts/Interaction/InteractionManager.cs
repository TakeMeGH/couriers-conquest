using System.Collections.Generic;
using UnityEngine;

namespace CC.Interaction
{
    public enum InteractionType { None = 0, PickUp, Shop, Talk };

    public class InteractionManager : MonoBehaviour
    {
        [SerializeField] private InputReader _inputReader = default;

        public InteractionType currentInteractionType { get; private set; }

        List<Interaction> _potentialInteractions = new List<Interaction>();

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

            currentInteractionType = _potentialInteractions[0].type;
            Debug.Log(_potentialInteractions[0].type + " " + _potentialInteractions[0].interactableObject);

            switch (_potentialInteractions[0].type)
            {
                case InteractionType.Shop:
                    break;

                case InteractionType.Talk:
                    break;

                case InteractionType.PickUp:
                    break;
            }
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
            Interaction newPotentialInteraction = new Interaction(obj);

            if (newPotentialInteraction.type != InteractionType.None)
            {
                _potentialInteractions.Insert(0, newPotentialInteraction); // insert first
            }
        }

        private void RemovePotentialInteraction(GameObject obj)
        {
            for (int i = 0; i < _potentialInteractions.Count; i++)
            {
                if (_potentialInteractions[i].interactableObject == obj)
                {
                    _potentialInteractions.RemoveAt(i);
                    break;
                }
            }
        }

    }

}
