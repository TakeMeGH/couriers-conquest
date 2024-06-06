using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CC
{
    public class WalkNPC : MonoBehaviour
    {
        [SerializeField] Transform[] _waypoints;
        [SerializeField] float _speed;
        [SerializeField] Animator _animator;
        [SerializeField] float _stopDistance;
        [SerializeField] float _idleTime;
        Rigidbody _rigidbody;
        float _currentIdleTime = 100;
        bool _isWalking;
        bool _isIdling = false;
        Vector3 _direction;
        int _currentIndex = 0;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();

            StartWalk();
        }

        // Update is called once per frame
        void Update()
        {
            if (_isWalking)
            {
                if (CheckIsArrive())
                {
                    StartIdle();
                }
                else
                {
                    _rigidbody.AddForce(_direction * _speed - GetHorizontalVelocity(), ForceMode.VelocityChange);
                }
            }
            else
            {
                _currentIdleTime += Time.deltaTime;
                if (_currentIdleTime >= _idleTime)
                {
                    StartWalk();
                }
            }
        }

        void StartIdle()
        {
            _rigidbody.velocity = Vector3.zero;
            _isIdling = true;
            _isWalking = false;
            _currentIdleTime = 0;

            _animator.Play("Idle");


            _currentIndex = (_currentIndex + 1) % _waypoints.Length;

        }


        void StartWalk()
        {
            _isIdling = false;
            _isWalking = true;
            _animator.Play("Walk");
            transform.LookAt(_waypoints[_currentIndex].position);
            _direction = _waypoints[_currentIndex].position - transform.position;

        }

        bool CheckIsArrive()
        {
            float _distance = Vector3.Distance(_waypoints[_currentIndex].position, transform.position);
            if (_distance <= _stopDistance)
            {
                return true;
            }
            return false;
        }

        protected Vector3 GetHorizontalVelocity()
        {
            Vector3 horizontalVelocity = _rigidbody.velocity;

            horizontalVelocity.y = 0f;

            return horizontalVelocity;
        }

    }
}
