using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using CC.Core.Daytime;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

namespace CC.Core.Save.UI
{
    public class LoadButton : MonoBehaviour
    {
        [SerializeField] int slot;
        [SerializeField] GameData _data;
        [SerializeField] SaveDataHandler _handler;
        [Header("Display")]
        [SerializeField] TextMeshProUGUI _saveDateText;
        [SerializeField] TextMeshProUGUI _dayText;
        [SerializeField] TextMeshProUGUI _timeText;
        [Header("Null/NotNull")]
        [SerializeField] GameObject _nullField;
        [SerializeField] GameObject _notNullObject;
        public void PopulateButton()
        {
            string path = Application.persistentDataPath + "/GameData/Save" + slot;
            if (!File.Exists(path))
            {
                Debug.Log("File on slot not found");
                DisplayNull();
                return;
            }
            try
            {
                _data = JsonConvert.DeserializeObject<GameData>(File.ReadAllText(path));
                OnDisplay();
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        void DisplayNull()
        {
            _notNullObject.SetActive(false);
            _nullField.SetActive(true);
            GetComponent<Button>().interactable = false;
        }

        void OnDisplay()
        {
            DayTimeData _model = ((JObject)_data.saveableModelSOs[0]).ToObject<DayTimeData>();
            _nullField.SetActive(false);
            _notNullObject.SetActive(true);
            _dayText.text = "Day " + _model.day.ToString();
            _timeText.text = string.Format("{0:00}:{1:00}", _model.time / 60, _model.time % 60);
        }

        public void loadOnClick()
        {
            _handler.LoadGame(this, slot);
        }
    }
}
