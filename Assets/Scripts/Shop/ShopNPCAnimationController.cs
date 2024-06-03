using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace CC
{
    public class ShopNPCAnimationController : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField] float _idleTime;
        [SerializeField] float _actionTime;
        bool _isRoutineAnimationRuning;
        bool _listenerAvailable;
        float _currentIdleTime;
        float _currentActionTime;
        DialogueRunner _dialogueRunner;

        private void Start()
        {
            if (_dialogueRunner == null) _dialogueRunner = FindObjectOfType<DialogueRunner>();
            RoutineAnimation();
        }

        private void Update()
        {
            if (_isRoutineAnimationRuning)
            {
                if (_currentActionTime > 0)
                {
                    _currentActionTime -= Time.deltaTime;
                    if (_currentActionTime <= 0)
                    {
                        _animator.Play("Idle");
                    }
                }
                else
                {
                    if (_currentIdleTime > 0)
                    {
                        _currentIdleTime -= Time.deltaTime;
                    }
                    else
                    {
                        RoutineAnimation();
                    }
                }
            }
        }

        public void Interact()
        {
            _animator.Play("Interact");
            _isRoutineAnimationRuning = false;
            _dialogueRunner.onDialogueComplete.AddListener(RoutineAnimation);
            _listenerAvailable = true;
        }

        private void RoutineAnimation()
        {
            if (_listenerAvailable == true)
            {
                _dialogueRunner.onDialogueComplete.RemoveListener(RoutineAnimation);
                _listenerAvailable = false;
            }
            _isRoutineAnimationRuning = true;
            _animator.Play("Action");
            _currentIdleTime = _idleTime;
            _currentActionTime = _actionTime;
        }
    }
}
