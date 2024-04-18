using System.Collections;
using System.Collections.Generic;
using CC.Events;
using UnityEngine;

public class WeightManager : MonoBehaviour
{
    [SerializeField] float _weightTreshold;
    [SerializeField][field: Range(0f, 1f)] float _weightMultiplier;
    [SerializeField] FloatEventChannelSO _onWeightUpdated;
    Rigidbody _rigidbody;
    float _defaultMass;
    float _currentWeight;

    private void OnEnable()
    {
        _onWeightUpdated.OnEventRaised += UpdateCurrentWeight;
    }

    private void OnDisable()
    {
        _onWeightUpdated.OnEventRaised -= UpdateCurrentWeight;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _defaultMass = _rigidbody.mass;
    }

    void UpdateCurrentWeight(float value)
    {
        _currentWeight = value;
        UpdateMass();
    }

    void UpdateMass()
    {
        if (_currentWeight >= _weightTreshold)
        {
            float _mLoad = (_currentWeight - _weightTreshold) * _weightMultiplier;
            _rigidbody.mass = _mLoad + _defaultMass;
        }
    }
}
