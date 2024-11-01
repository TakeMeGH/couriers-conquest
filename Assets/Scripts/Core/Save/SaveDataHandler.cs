using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using Newtonsoft.Json.Linq;
using CC.Event;
using CC.Core.Save.UI;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace CC.Core.Save
{
    public class SaveDataHandler : MonoBehaviour
    {
        [SerializeField] List<ASavableModel> _dataModels; //slot 0 untuk DayTime DataModel
        [SerializeField] GameData _gameData;
        //[SerializeField] float _intervalSecond = 300;
        [SerializeField] bool autoSave;
        [Header("Load")]
        [SerializeField] bool _loadOnAwake;
        [SerializeField] SenderDataEventChannelSO _OnAwakeLoad;
        [Header("Save")]
        [SerializeField] Button _saveButton;
        [Header("Events")]
        [SerializeField] SenderDataEventChannelSO _OnBeforeSave;
        [SerializeField] SenderDataEventChannelSO _OnLoadFinished;
        [SerializeField] SenderDataEventChannelSO _OnSaveFinished;
        [SerializeField] SenderDataEventChannelSO _onNewGameFinished;
        float timeIndicator;
        private void Awake()
        {
            if (_loadOnAwake) _OnAwakeLoad?.raiseEvent(this, null);
        }
        private void Start()
        {
            Debug.Log("Save Location = " + Application.persistentDataPath + "/GameData/Save");
        }
        private void Update()
        {
            //timeIndicator += Time.deltaTime;
            //if (timeIndicator >= _intervalSecond) AutoSave(0);
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    StartCoroutine(LoadData(0));
            //}
        }

        #region "To be Called On Event"
        public void LoadGame(Component sender, object slot)
        {
            if (slot is int) { LoadData((int)slot); }
        }

        public void AutoSave(Component sender, object slot)
        {
            Debug.Log("Auto saving..");
            if (autoSave && slot is int) StartCoroutine(ForceSaveData((int)slot));
        }

        public void QuestSave(bool setAuto)
        {
            if (autoSave)
            {
                autoSave = setAuto;
                StartCoroutine(ForceSaveData(1));
            }
            else
            {
                autoSave = setAuto;
                StartCoroutine(ForceSaveData(1));
            }
            if (_saveButton != null) _saveButton.interactable = autoSave;
        }

        public void ManualSave(Component sender, object slot)
        {
            if (slot is int) StartCoroutine(ForceSaveData((int)slot));
        }
        #endregion "To be Called On Event"

        IEnumerator ForceSaveData(int slot)
        {
            Debug.Log("Saving..");
            yield return null;
            _OnBeforeSave?.raiseEvent(this, slot);
            string path = Application.persistentDataPath + "/GameData/Save" + slot;
            Debug.Log(path);
            _gameData = new GameData();
            _gameData.saveTime = DateTime.Now;
            foreach (var model in _dataModels)
            {
                _gameData.saveableModelSOs.Add(model.Save());
            }
            if (File.Exists(path))
            {
                try
                {
                    Debug.Log("Exist, overwritting it");
                    File.Delete(path);
                    using FileStream stream = File.Create(path);
                    stream.Close();
                    File.WriteAllText(path, JsonConvert.SerializeObject(_gameData, Formatting.Indented, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
            else
            {
                try
                {
                    Debug.Log("NotExist,writing new one");
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    using FileStream stream = File.Create(path);
                    stream.Close();
                    File.WriteAllText(path, JsonConvert.SerializeObject(_gameData, Formatting.Indented, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
            yield return null;
            Debug.Log("save finished");
            _OnSaveFinished?.raiseEvent(this, null);
        }

        async void LoadData(int slot)
        {
            string path = Application.persistentDataPath + "/GameData/Save" + slot;
            Debug.Log("trying to load" + path);

            if (!File.Exists(path))
            {
                Debug.Log("File on slot not found");
                return;
            }
            try
            {
                GameData LoadedGameData = JsonConvert.DeserializeObject<GameData>(File.ReadAllText(path));
                var taskList = new List<Task>();
                for (int i = 0; i < _dataModels.Count; i++)
                {
                    object loadedData = LoadedGameData.saveableModelSOs[i];
                    //_dataModels[i].Load(loadedData);
                    taskList.Add(populateDataModels(_dataModels[i], loadedData));
                }
                await Task.WhenAll(taskList);
                Debug.Log("Load Finishes");
                _OnLoadFinished?.raiseEvent(this, null);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        async Task populateDataModels(ASavableModel model, object data)
        {
            model.Load(data);
            await Task.Yield();
        }

        public async void initNewGame()
        {
            foreach (var model in _dataModels) model.SetDefaultValue();
            await Task.Yield();
            _onNewGameFinished?.raiseEvent(this, null);
        }
    }

    [System.Serializable]
    public class GameData
    {
        [Header("Meta")]
        public DateTime saveTime;
        [Header("Data")]
        public List<object> saveableModelSOs = new();
    }
}
