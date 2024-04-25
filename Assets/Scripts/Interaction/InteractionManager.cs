using System;
using System.Collections.Generic;
using System.Linq;
using CC.Events;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CC.Interaction
{

    public class InteractionManager : MonoBehaviour
    {
        [SerializeField] InputReader _inputReader = default;
        [SerializeField] InteractEventUISO _addInteractionEvent;
        [SerializeField] IntEventChannelSO _removeInteractionEvent;
        List<InteracionInstance> _potentialInteractions = new List<InteracionInstance>();
        CustomInterractables _currentInteraction;
        int _currentInteractionIndex = 0;
        private void OnEnable()
        {
            _inputReader.InteractPerformed += OnInteractionButtonPress;
            _inputReader.ScrollInteracionPerformed += OnScrollUIPerformed;
        }

        private void OnDisable()
        {
            _inputReader.InteractPerformed -= OnInteractionButtonPress;
            _inputReader.ScrollInteracionPerformed -= OnScrollUIPerformed;

        }

        private void Update()
        {
            // Debug.Log(_currentInteractionIndex + " " + _currentInteraction.transform.parent.gameObject.name);
        }

        private void Start()
        {
            _inputReader.DisableScrollInteracionInput();
        }
        private void OnInteractionButtonPress()
        {
            if (_potentialInteractions.Count == 0)
                return;

            if (_currentInteraction.transform.parent.tag == "Pickable")
            {
                _currentInteraction.Interact();
                RemoveCurrentInteraction();
            }
            else
            {
                _currentInteraction.Interact();

            }

        }

        void OnScrollUIPerformed(float _scrollValue)
        {
            int scroll = 0;
            if (_scrollValue < 0) // scroll down
            {
                scroll = 1;
            }
            if (_scrollValue > 0) // scroll up
            {
                scroll = -1;
            }
            int _nextIndex = _currentInteractionIndex + scroll;
            if (_nextIndex >= 0 && _nextIndex < _potentialInteractions.Count)
            {
                _currentInteraction = _potentialInteractions[_nextIndex]._customInterractables;
                _currentInteractionIndex = _nextIndex;
            }
            // Debug.Log(_currentInteractionIndex + " " + _currentInteraction.transform.parent.gameObject.name);

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
            if (obj.TryGetComponent<CustomInterractables>(out CustomInterractables currentInteraction))
            {
                if (_potentialInteractions.Count == 0)
                {
                    _currentInteraction = currentInteraction;
                    _currentInteractionIndex = 0;
                    _inputReader.EnableScrollInteracionInput();
                }
                var _instanceUIId = _addInteractionEvent.RaiseEvent(currentInteraction.GetIcon(),
                        currentInteraction.GetAmount(), currentInteraction.GetName());
                _potentialInteractions.Add(new InteracionInstance(currentInteraction, _instanceUIId)); // insert last
            }
        }

        private void RemovePotentialInteraction(GameObject obj)
        {
            if (obj.TryGetComponent<CustomInterractables>(out CustomInterractables currentInteraction))
            {
                for (int i = 0; i < _potentialInteractions.Count; i++)
                {
                    if (_potentialInteractions[i]._customInterractables == currentInteraction)
                    {
                        _removeInteractionEvent.RaiseEvent(_potentialInteractions[i]._instanceUIId);
                        _potentialInteractions.RemoveAt(i);

                        if (_currentInteractionIndex >= i)
                        {
                            _currentInteractionIndex = Math.Max(0, _currentInteractionIndex - 1);
                            if (_potentialInteractions.Any()) _currentInteraction = _potentialInteractions[_currentInteractionIndex]._customInterractables;
                        }

                        break;
                    }
                }
            }

            if (_potentialInteractions.Count == 0)
            {
                _inputReader.DisableScrollInteracionInput();
            }

        }

        private void RemoveCurrentInteraction()
        {
            if (_currentInteractionIndex == -1)
            {
                return;
            }
            _removeInteractionEvent.RaiseEvent(_potentialInteractions[_currentInteractionIndex]._instanceUIId);
            _potentialInteractions.RemoveAt(_currentInteractionIndex);
            int _newIndex = _currentInteractionIndex;
            if (_currentInteractionIndex > 0)
            {
                _newIndex = _currentInteractionIndex - 1;
            }

            if (_potentialInteractions.Any())
            {
                _currentInteraction = _potentialInteractions[_newIndex]._customInterractables;
                _currentInteractionIndex = _newIndex;

            }
        }


    }

    public class InteracionInstance
    {
        public CustomInterractables _customInterractables;
        public int _instanceUIId;

        public InteracionInstance(CustomInterractables customInterractables, int instanceUIId)
        {
            _customInterractables = customInterractables;
            _instanceUIId = instanceUIId;
        }
    }

}
