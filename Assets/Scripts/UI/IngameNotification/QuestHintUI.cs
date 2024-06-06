using CC.Quest.UI;
using CC.RuntimeAnchors;
using TMPro;
using UnityEngine;

namespace CC.UI.Notification
{
    public class QuestHintUI : MonoBehaviour
    {
        [Header("Visuals")]
        [SerializeField] TextMeshProUGUI _questObjective;
        [SerializeField] TextMeshProUGUI _distanceToObjective;
        [SerializeField] GameObject _Visuals;
        [SerializeField] TransformAnchor _playerTransformAnchor = default;

        [Header("Param")]
        [SerializeField] bool showHint;
        [SerializeField] GameObject _targetObjective;
        Transform _player;
        private void OnEnable()
        {
            _playerTransformAnchor.OnAnchorProvided += SetPlayerTransfrom;
        }

        private void OnDisable()
        {
            _playerTransformAnchor.OnAnchorProvided -= SetPlayerTransfrom;
        }


        public void Hints(Component sender, object data)
        {
            if (data is HintData)
            {
                HintData _data = (HintData)data;
                _questObjective.text = _data.Objective;
                _targetObjective = _data.Destination;
                showHint = true;
                _Visuals.SetActive(true);
            }
        }

        private void Update()
        {
            if (showHint && _player != null) updateDistance();
        }

        void updateDistance()
        {
            _distanceToObjective.text = ((int)Vector3.Distance(_player.transform.position, _targetObjective.transform.position)).ToString() + "M";

        }

        public void stopHint()
        {
            showHint = false;
            _Visuals.SetActive(false);
        }

        void SetPlayerTransfrom()
        {
            _player = _playerTransformAnchor.Value;
        }

        private void OnDestroy()
        {
            stopHint();
        }

    }
}
