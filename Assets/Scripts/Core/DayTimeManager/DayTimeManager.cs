using CC.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Core.Daytime
{
    public class DayTimeManager : MonoBehaviour
    {
        [Header("Data Model")]
        [SerializeField] DayTimeDataModel _model;
        [Header("Config")]
        [SerializeField] float _secToMin = 1;
        [Header("Debug")]
        [SerializeField] int _time;
        [SerializeField] int _day;
        [SerializeField] VoidEventChannelSO _OnTimeUpdate;

        float _recTime;

        private void Start()
        {
            _day = _model.getDay();
            _time = _model.getTime();
        }

        private void Update()
        {
            updateTime();
        }

        private void FixedUpdate()
        {
            
        }

        void updateTime()
        {
            _recTime += Time.deltaTime;
            if (_recTime > _secToMin)
            {
                Debug.Log("UpdatingTime");
                if(_time >= 1440){ _model.OnTimeUpdate(0); _model.OnDayUpdate(_model.getDay() + 1); _day = _model.getDay(); }
                else _model.OnTimeUpdate(_time + 1);
                _recTime = 0;
                _time = _model.getTime();
                _OnTimeUpdate?.RaiseEvent();
            }
        }
    }
}
