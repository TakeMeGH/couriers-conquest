using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using Newtonsoft.Json.Linq;

namespace CC.Core.Save
{
    public class SaveDataHandler : MonoBehaviour
    {
        [SerializeField] List<ASavableModel> _dataModels;
        [SerializeField] GameData _gameData;
        [SerializeField] float _intervalSecond = 300;
        [SerializeField] bool autoSave;
        float timeIndicator;
        private void Start()
        {
            //ForceSaveData(0);
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
            if (slot is int) StartCoroutine(LoadData((int)slot));
        }

        public void AutoSave(Component sender, object slot)
        {
            if (slot is int) StartCoroutine(ForceSaveData((int)slot));
        }

        public void ManualSave(Component sender, object slot)
        {
            if (slot is int) StartCoroutine(ForceSaveData((int)slot));
        }
        #endregion "To be Called On Event"

        IEnumerator ForceSaveData(int slot)
        {
            string path = Application.persistentDataPath + "/GameData/Save" + slot;
            Debug.Log(path);
            _gameData = new GameData();
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
            return null;
        }

        IEnumerator LoadData(int slot)
        {
            string path = Application.persistentDataPath + "/GameData/Save" + slot;
            Debug.Log("trying to load" + path);

            if (!File.Exists(path))
            {
                Debug.Log("File on slot not found");
                return null;
            }
            try
            {
                GameData LoadedGameData = JsonConvert.DeserializeObject<GameData>(File.ReadAllText(path));
                for (int i = 0; i < _dataModels.Count; i++)
                {
                    object loadedData = LoadedGameData.saveableModelSOs[i];
                    Debug.Log(loadedData.GetType());
                    _dataModels[i].Load(loadedData);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
            return null;
        }
    }

    [System.Serializable]
    public class GameData
    {
        [Header("Meta")]
        [Header("Data")]
        public List<object> saveableModelSOs = new();
    }
}
