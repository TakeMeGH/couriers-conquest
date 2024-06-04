using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CC.Core.Save.UI
{
    public class LoadHandler : MonoBehaviour
    {
        [SerializeField] List<LoadButton> buttonList;
        [SerializeField] SaveDataHandler _handler;
        [SerializeField] int _slot;
        public void tryFindSave()
        {
            foreach (var button in buttonList)
            {
                button.PopulateButton();
            }
        }

        public void goToSceneName(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void goToSceneNameAsync(string name)
        {
            loadAsyncByName(name);
        }

        IEnumerator loadAsyncByName(string name)
        {
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(name);
            while (!loadSceneAsync.isDone)
            {
                yield return null;
            }
        }

        public void selectSlot(int slot)
        {
            _slot = slot;
        }

        public void HandleLoad()
        {
            _handler.LoadGame(this, _slot);
        }
    }
}
