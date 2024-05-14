using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CC.Events;
using Unity.VisualScripting;
using UnityEngine.UI;
using System;
namespace CC
{
    public class InteractionUIManager : MonoBehaviour
    {
        [SerializeField] GameObject _contentObject;
        [SerializeField] InteractEventUISO _addInteractionEvent;
        [SerializeField] IntEventChannelSO _removeInteractionEvent;
        [SerializeField] float _scrollSpeed;
        [SerializeField] InputReader _inputReader;
        List<InteractPanel> _unactiveInteractionPanel = new List<InteractPanel>();

        List<InteractPanel> _activeInteractionPanel = new List<InteractPanel>();
        ScrollRect _scrollRect;
        [SerializeField] int _currentIndex = -1;

        private void OnEnable()
        {
            _addInteractionEvent.OnEventRaised += AddInteraction;
            _removeInteractionEvent.OnEventRaised += RemoveInteraction;
            _inputReader.ScrollInteracionPerformed += OnScrollUIPerformed;
        }

        private void OnDisable()
        {
            _addInteractionEvent.OnEventRaised -= AddInteraction;
            _removeInteractionEvent.OnEventRaised -= RemoveInteraction;
            _inputReader.ScrollInteracionPerformed -= OnScrollUIPerformed;

        }

        private void Start()
        {
            _scrollRect = GetComponent<ScrollRect>();
            for (int i = 0; i < _contentObject.transform.childCount; i++)
            {
                _unactiveInteractionPanel.Add(_contentObject.transform.GetChild(i).GetComponent<InteractPanel>());
            }
        }
        void OnScrollUIPerformed(float _scrollValue)
        {
            int scroll = 0;
            if (_scrollValue < 0) // scroll down
            {
                scroll = -1;
            }
            if (_scrollValue > 0) // scroll up
            {
                scroll = 1;
            }

            if (scroll != 0)
            {
                float contentHeight = _scrollRect.content.sizeDelta.y;
                float contentShift = _scrollSpeed * scroll;
                _scrollRect.verticalNormalizedPosition += contentShift / contentHeight;

                int _newIndex = _currentIndex + (-1 * scroll);

                if (_newIndex >= 0 && _newIndex < _activeInteractionPanel.Count)
                {
                    _activeInteractionPanel[_currentIndex].DisableSelected();
                    _activeInteractionPanel[_newIndex].EnableSelected();
                    _currentIndex = _newIndex;
                }

            }
        }


        int AddInteraction(Sprite _icon, int _amount, string _interactionName)
        {
            InteractPanel _currentInteraction = _unactiveInteractionPanel[0];
            _unactiveInteractionPanel.RemoveAt(0);

            _currentInteraction.Init(_icon, _amount, _interactionName);

            if (_activeInteractionPanel.Count == 0)
            {
                _currentInteraction.EnableSelected();
                _currentIndex = 0;
            }

            MoveLast(_currentInteraction.transform);
            _currentInteraction.gameObject.SetActive(true);
            _activeInteractionPanel.Add(_currentInteraction);
            return _currentInteraction.GetInstanceID();
        }

        void MoveLast(Transform child)
        {
            int index = _activeInteractionPanel.Count;
            child.SetSiblingIndex(index);
        }

        void MoveToIndex(Transform child, int index)
        {
            child.SetSiblingIndex(index);
        }


        void RemoveInteraction(int _instanceUIId)
        {
            int _removeIndex = -1;
            for (int i = 0; i < _activeInteractionPanel.Count; i++)
            {
                InteractPanel _currentInteraction = _activeInteractionPanel[i];


                if (_currentInteraction.GetInstanceID() == _instanceUIId)
                {
                    _activeInteractionPanel.RemoveAt(i);
                    _unactiveInteractionPanel.Add(_currentInteraction);
                    _currentInteraction.DisableSelected();
                    _currentInteraction.gameObject.SetActive(false);
                    _removeIndex = i;
                    break;
                }
            }
            if (_currentIndex >= _removeIndex)
            {
                int _newSelectedIndex = Math.Max(0, _removeIndex - 1);
                _currentIndex = _newSelectedIndex;

                if (_activeInteractionPanel.Count > 0)
                {
                    _activeInteractionPanel[_currentIndex].EnableSelected();
                }
            }

            ResetPosition();
        }
        void ResetPosition()
        {
            for (int i = 0; i < _activeInteractionPanel.Count; i++)
            {
                MoveToIndex(_activeInteractionPanel[i].transform, i);
            }
        }
    }
}
