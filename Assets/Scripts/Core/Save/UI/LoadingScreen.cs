using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CC
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] string _targetSceneName;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(loadAsyncByName(_targetSceneName));
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        IEnumerator loadAsyncByName(string name)
        {
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(name);
            while (!loadSceneAsync.isDone)
            {
                yield return null;
            }
        }
    }
}
