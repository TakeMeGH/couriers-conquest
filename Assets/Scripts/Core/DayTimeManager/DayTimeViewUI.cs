using CC.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CC.Core.Daytime
{
    public class DayTimeViewUI : MonoBehaviour
    {
        [SerializeField] DayTimeDataModel _model;
        [SerializeField] TextMeshProUGUI _dayText;
        [SerializeField] TextMeshProUGUI _timeText;
        [SerializeField] VoidEventChannelSO _channelSO;

        private void OnEnable()
        {
            _channelSO.OnEventRaised += OnDisplay;
        }
        private void OnDisable()
        {
            _channelSO.OnEventRaised -= OnDisplay;
        }

        private void LateUpdate()
        {
            
        }

        void OnDisplay()
        {
            Debug.Log("displaying");
            _dayText.text = "Day " + _model.getDay();
            _timeText.text = string.Format("{0:00}:{1:00}", _model.getTime() / 60, _model.getTime() % 60);
        }
    }
}
