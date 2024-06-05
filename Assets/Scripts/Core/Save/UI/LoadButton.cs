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
        [SerializeField] LoadHandler _loadHandler;
        [Header("Display")]
        [SerializeField] Image _lastCaptureImage;
        [SerializeField] TextMeshProUGUI _saveDateText;
        [SerializeField] TextMeshProUGUI _dayText;
        [SerializeField] TextMeshProUGUI _timeText;
        [Header("Null/NotNull")]
        [SerializeField] GameObject _nullField;
        [SerializeField] GameObject _notNullObject;
        public void PopulateButton(bool isIgnoreUnavailableSlot = true)
        {
            string path = Application.persistentDataPath + "/GameData/Save" + slot;
            if (!File.Exists(path))
            {
                Debug.Log("File on slot not found");
                DisplayNull(isIgnoreUnavailableSlot);
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

        void DisplayNull(bool isIgnoreUnavailableSlot = true)
        {
            _notNullObject.SetActive(false);
            _nullField.SetActive(true);
            GetComponent<Button>().interactable = isIgnoreUnavailableSlot;
        }

        void OnDisplay()
        {
            DayTimeData _model = ((JObject)_data.saveableModelSOs[0]).ToObject<DayTimeData>();
            _nullField.SetActive(false);
            _notNullObject.SetActive(true);
            _lastCaptureImage.sprite = loadSprite();
            _dayText.text = "Day " + _model.day.ToString();
            _timeText.text = string.Format("{0:00}:{1:00}", _model.time / 60, _model.time % 60);
            _saveDateText.text = _data.saveTime.ToString();
        }

        Sprite loadSprite()
        {
            string path = Application.persistentDataPath + "/GameData/SaveShot" + slot;
            Texture2D loaded = LoadImage(path);
            if(loaded != null) return Sprite.Create(loaded, new Rect(0.0f, 0.0f, LoadImage(path).width, LoadImage(path).height), new Vector2(0.5f, 0.5f), 100.0f);
            else return null;
        }

        public Texture2D LoadImage(string path)
        {
            if (File.Exists(path))
            {
                byte[] bytes = File.ReadAllBytes(path);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(bytes);

                return tex;
            }
            else
            {
                return null;
            }
        }

        public void loadOnClick()
        {
            _loadHandler.selectSlot(slot);
        }
    }
}
