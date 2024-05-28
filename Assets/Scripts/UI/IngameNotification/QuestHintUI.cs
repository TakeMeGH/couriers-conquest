using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CC.UI.Notification
{
    public class QuestHintUI : MonoBehaviour
    {
        [Header("Visuals")]
        [SerializeField] TextMeshProUGUI _QuestTitle;
        [SerializeField] TextMeshProUGUI _QuestObjective;
        [SerializeField] TextMeshProUGUI _DistanceToObjective;
        [SerializeField] GameObject _Visuals;

        [Header("Param")]
        [SerializeField] bool showHint;
        [SerializeField] GameObject _targetObjective;
        [SerializeField] GameObject _player;

        public void Hints(Component sender, object data)
        {
            if(data is QuestHintData)
            {
                QuestHintData _data = (QuestHintData)data;
                _QuestTitle.text = _data.questName;
                _QuestObjective.text = _data.questObjective;
                _targetObjective = _data.targetObjective;
                showHint = true;
                _Visuals.SetActive(true);
            }
        }

        private void Update()
        {
            if (showHint) updateDistance();
        }

        void updateDistance()
        {
            _DistanceToObjective.text = Vector3.Distance(_player.transform.position, _targetObjective.transform.position).ToString("F2");
        }

        public void stopHint()
        {
            showHint = false;
            _Visuals.SetActive(false);
        }


    }
    public class QuestHintData
    {
        public readonly string questName;
        public readonly string questObjective;
        public readonly GameObject targetObjective;
        public QuestHintData(string _name, string _objective, GameObject _target)
        {
            questName = _name;
            questObjective = _objective;
            targetObjective = _target;
        }
    }
}
