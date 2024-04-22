    using CC.Core.Data.Dynamic;
using CC.Event;
using UnityEngine;

namespace CC.Core
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] PlayerStateSO _stateModel;
        [SerializeField] GameObject _playerPrefab;
        [SerializeField] GameObject _player;
        [SerializeField] Vector3 _positionBuffer;

        // Start is called before the first frame update
        void Start()
        {
            _player = Instantiate(_playerPrefab,_stateModel.GetSavedPosition() + _positionBuffer,Quaternion.identity);
        }
        public void savePos(Component sender, object data)
        {
            _stateModel.SaveCurrentPosition(_player.transform.position);
        }
    }
}
