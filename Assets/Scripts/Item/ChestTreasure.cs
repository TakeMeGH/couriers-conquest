using CC.Events;
using CC.Interaction;
using CC.Inventory;
using UnityEngine;

namespace CC.Items
{
    public class ChestTreasure : MonoBehaviour
    {
        [SerializeField] ChestDataModel _chestData;
        [SerializeField] CustomInterractables _customInteractables;
        [SerializeField] OnUpdateCurrencyEventChannel _onUpdateCurrency;
        [SerializeField] Animator _animator;
        [SerializeField] Vector3 _offset;
        [SerializeField] Vector3 _randomVector3;
        public bool IsCanOpen = true;

        public void Initialize()
        {
            if (_chestData.CurrentChestData.IsAlreadyOpen)
            {
                Destroy(_customInteractables);
                _animator.Play("IdleOpen");
            }
            else
            {
                _animator.Play("IdleClosed");
            }
        }

        public void Interact()
        {
            foreach (ChestValue item in _chestData.itemChest)
            {
                AttempToDrop(item);
            }
            _chestData.CurrentChestData.IsAlreadyOpen = true;
            _onUpdateCurrency.RaiseEvent(_chestData.Gold);

            _customInteractables.transform.position = Vector3.zero;
            Destroy(_customInteractables, 0.2f);
            _animator.Play("Open");
        }
        private void Start()
        {
            Initialize();
        }

        public void AttempToDrop(ChestValue item)
        {
            if (item == null)
            {
                Debug.Log("Could not find Item in Dictionary to add to drop");
                return;
            }

            GameObject dropPrefab = ObjectPooling.SharedInstance.GetPooledObject(PoolObjectType.Item);

            if (dropPrefab != null)
            {
                dropPrefab.transform.position = transform.position + _offset + GenerateRandomVector();
                dropPrefab.transform.rotation = transform.rotation;
                dropPrefab.SetActive(true);
            }

            Rigidbody rb = dropPrefab.GetComponent<Rigidbody>();
            if (rb != null) rb.velocity = transform.up * 4;

            ItemPickup ip = dropPrefab.GetComponentInChildren<ItemPickup>();
            ip.isDropItem = true;
            if (ip != null)
            {
                ip.item = item.item;
                ip.amount = item.amount;
            }
        }

        Vector3 GenerateRandomVector()
        {
            Vector3 randomVector = new Vector3(Random.Range(-_randomVector3.x, _randomVector3.x),
                                       Random.Range(-_randomVector3.y, _randomVector3.y),
                                       Random.Range(-_randomVector3.z, _randomVector3.z));

            return randomVector;
        }
    }
}
