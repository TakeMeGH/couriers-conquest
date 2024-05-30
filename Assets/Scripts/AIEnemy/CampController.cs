using System.Collections.Generic;
using CC.Enemy;
using UnityEngine;

namespace CC
{
    public class CampController : MonoBehaviour
    {
        [SerializeField] bool _isUsingChest;
        [SerializeField] bool _isUsingBorder;
        [SerializeField] List<GameObject> _borders;
        [SerializeField] List<EnemyController> _enemyControllers;

        float _cooldownTime = 2f;
        float _lastTime = -5f;


        private void Start()
        {
            for (int i = 0; i < _enemyControllers.Count; i++)
            {
                _enemyControllers[i].gameObject.SetActive(true);
                _enemyControllers[i].OnPlayerDetected += OnPlayerDetected;
            }
        }

        void OnPlayerDetected()
        {
            float currentTime = Time.time;
            if (currentTime - _lastTime < _cooldownTime)
            {
                return;
            }

            _lastTime = currentTime;
            for (int i = 0; i < _enemyControllers.Count; i++)
            {
                if(!_enemyControllers[i].isActiveAndEnabled) continue;
                _enemyControllers[i].PlayerInRange();
            }
        }
    }
}
