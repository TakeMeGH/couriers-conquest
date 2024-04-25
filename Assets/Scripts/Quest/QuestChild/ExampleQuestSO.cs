using System.Collections.Generic;
using UnityEngine;

namespace CC.Quest
{
    [CreateAssetMenu(menuName = "Data/Statics/Quest/ExampleQuest")]
    public class ExampleQuestSO : AQuest
    {
        [SerializeField] questPrefab[] _questPrefabs;
        [SerializeField] bool _pickedUp;
        List<GameObject> _instantiatedPrefabs = new();
        public override void OnQuestStarted(Component sender, object data)
        {
            foreach (var prefab in _questPrefabs)
            {
                var temp = Instantiate(prefab.prefab, prefab.position, Quaternion.identity);
                if (temp != null) _instantiatedPrefabs.Add(temp);
            }
            Debug.Log("quest has started");
        }
        public override void OnQuestProgress(Component sender, object data)
        {
            _pickedUp = true;
            Debug.Log("picked up");
        }
        public override void OnQuestFinished(Component sender, object data)
        {
            clearAllPrefab();
            Debug.Log("quest has finished");
        }
        public override void OnQuestCancelled(Component sender, object data)
        {
            clearAllPrefab();
            Debug.Log("quest has cancelled");
        }

        public void clearAllPrefab()
        {
            if (_instantiatedPrefabs != null)
            {
                foreach (var temp in _instantiatedPrefabs)
                {
                    if (temp != null) Destroy(temp);
                }
                _instantiatedPrefabs.Clear();
            }
            //foreach (GameObject item in GameObject.FindGameObjectsWithTag("PayLoad"))
            //{
               // Destroy(item);
            //}
            //foreach (GameObject item in GameObject.FindGameObjectsWithTag("VPayload"))
            //{
               // item.SetActive(false);
            //}
        }

        public bool IsPickedUp()
        {
            return _pickedUp;
        }
    }

    [System.Serializable]
    public class questPrefab
    {
        public GameObject prefab;
        public Vector3 position;
    }
}
