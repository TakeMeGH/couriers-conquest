using System.Collections.Generic;
using CC.Enemy;
using CC.Event;
using CC.Items;
using UnityEngine;

namespace CC
{
    public class CampController : MonoBehaviour
    {
        [Header("Is Using")]
        [SerializeField] bool _isUsingChest;
        [SerializeField] bool _isUsingBorder;
        [Header("Component")]
        [SerializeField] List<GameObject> _borders;
        [SerializeField] List<EnemyController> _enemyControllers;
        [SerializeField] GameObject _enemyCamp;
        [SerializeField] ChestTreasure _chest;

        [Header("Data")]
        [SerializeField] CampDataModel _campDataModel;
        [SerializeField] bool _isBandit;
        [Header("Event")]
        [SerializeField] SenderDataEventChannelSO _onDayChange;
        List<EnemyController> _spawnedEnemyControllers;


        float _cooldownTime = 2f;
        float _lastTime = -5f;

        private void OnEnable()
        {
            _onDayChange.OnEventRaised.AddListener(OnDaychange);
        }

        private void OnDisable()
        {
            _onDayChange.OnEventRaised.RemoveListener(OnDaychange);
        }
        private void Start()
        {
            _spawnedEnemyControllers = new(new EnemyController[_enemyControllers.Count]);
            for (int i = 0; i < _spawnedEnemyControllers.Count; i++) _spawnedEnemyControllers[i] = null;
            SpawnEnemy();
            CheckCanOpenChest();
        }

        void OnPlayerDetected()
        {
            float currentTime = Time.time;
            if (currentTime - _lastTime < _cooldownTime)
            {
                return;
            }

            Vector3 position = transform.position;
            _lastTime = currentTime;
            for (int i = 0; i < _spawnedEnemyControllers.Count; i++)
            {
                if (_spawnedEnemyControllers[i] == null) continue;
                if (_spawnedEnemyControllers[i].IsPlayerInRange())
                {
                    position = _spawnedEnemyControllers[i].transform.position;
                }
                _spawnedEnemyControllers[i].PlayerInRange();
            }

            if (_isBandit)
            {
                AudioManager.instance.AudioPlayOneShot(AudioManager.instance.BanditNotice, position);
            }
            else
            {
                AudioManager.instance.AudioPlayOneShot(AudioManager.instance.GoblinNotice, position);
            }

        }

        bool CanRespawn()
        {
            for (int i = 0; i < _spawnedEnemyControllers.Count; i++)
            {
                if (_spawnedEnemyControllers[i] == null) continue;
                if (_spawnedEnemyControllers[i].IsPlayerInRange())
                {
                    return false;
                }
            }
            return true;
        }

        bool AnyActive()
        {
            for (int i = 0; i < _spawnedEnemyControllers.Count; i++)
            {
                if (_spawnedEnemyControllers[i] != null) return true;
            }
            return false;
        }

        void ResetActive()
        {
            _campDataModel.ResetEnemyAlive();
        }

        void SpawnEnemy()
        {
            for (int i = 0; i < _campDataModel.CurrentCampData.IsAlive.Count; i++)
            {
                if (_spawnedEnemyControllers[i] != null) continue;
                bool status = _campDataModel.CurrentCampData.IsAlive[i];
                if (status)
                {
                    GameObject instantiatedObject = Instantiate(_enemyControllers[i].gameObject, _enemyCamp.transform);
                    instantiatedObject.SetActive(true);
                    _spawnedEnemyControllers[i] = instantiatedObject.GetComponent<EnemyController>();
                    _spawnedEnemyControllers[i].OnPlayerDetected += OnPlayerDetected;
                    _spawnedEnemyControllers[i].OnEnemyDead += RefreshData;

                }
            }
        }

        void OnDaychange(Component component, object data)
        {
            _campDataModel.CurrentCampData.CurrentDay++;
            if (_campDataModel.CurrentCampData.CurrentDay >= _campDataModel.DayCountToSpawn &&
                CanRespawn())
            {
                _campDataModel.CurrentCampData.CurrentDay = 0;
                ResetActive();
                SpawnEnemy();
            }

            if (_campDataModel.CurrentCampData.CurrentDay >= _campDataModel.DayCountToSpawn) _campDataModel.CurrentCampData.CurrentDay = 0;
        }

        void RefreshData()
        {
            for (int i = 0; i < _campDataModel.CurrentCampData.IsAlive.Count; i++)
            {
                if (_spawnedEnemyControllers[i] == null)
                {
                    _campDataModel.CurrentCampData.IsAlive[i] = false;
                }
                else
                {
                    _campDataModel.CurrentCampData.IsAlive[i] = !_spawnedEnemyControllers[i].IsDead;
                }
            }
        }

        void CheckCanOpenChest()
        {
            if (!_isUsingChest) return;
            if (AnyActive())
            {
                _chest.IsCanOpen = false;
            }
            else
            {
                _chest.IsCanOpen = true;
            }
        }
    }
}
