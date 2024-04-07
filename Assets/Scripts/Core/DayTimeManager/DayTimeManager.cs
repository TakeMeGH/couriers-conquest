using CC.Events;
using UnityEngine;

namespace CC.Core.Daytime
{
    public class DayTimeManager : MonoBehaviour
    {
        [Header("Data Model")]
        [SerializeField] DayTimeDataModel _model;
        [Header("Config")]
        [SerializeField] float _secToMin = 1;
        [Header("Debug")]
        [SerializeField] int _time;
        [SerializeField] int _day;
        [SerializeField] VoidEventChannelSO _OnTimeUpdate;
        float _recTime;

        [Header("Gradient")]
        [SerializeField] Gradient _fogGrad;
        [SerializeField] Gradient _lightGrad;
        [SerializeField] Gradient _skyBoxGrad;
        [Header("Water Gradient")]
        [SerializeField] Gradient _waterGrad;
        [SerializeField] Gradient _foamGrad;

        [Header("Environtmentals")]
        [SerializeField] Light _directionalLight;
        [SerializeField] Material _skyBoxMaterial;
        [SerializeField] Material _waterMaterial;

        [Header("Variables")]
        [SerializeField] float _rotationSpeed;
        [SerializeField] float _currentTimeGrad;

        private void Start()
        {
            _day = _model.getDay();
            _time = _model.getTime();
        }

        private void Update()
        {
            updateTime();
            updateCycle();
            rotateSkybox();
        }

        void updateTime()
        {
            _recTime += Time.deltaTime;
            if (_recTime > _secToMin)
            {
                Debug.Log("UpdatingTime");
                if(_time >= 1440){ _model.OnTimeUpdate(0); _model.OnDayUpdate(_model.getDay() + 1); _day = _model.getDay(); }
                else _model.OnTimeUpdate(_time + 1);
                _currentTimeGrad = _model.getTime() / 1440f;
                Debug.Log(_model.getTime() / 1440f);
                _recTime = 0;
                _time = _model.getTime();
                _OnTimeUpdate?.RaiseEvent();
            }
        }

        void updateCycle()
        {
            float sunPos = Mathf.Repeat(_currentTimeGrad - 0.275f, 1f);
            _directionalLight.transform.rotation = Quaternion.Euler(sunPos * 360, 0f, 0f);
            RenderSettings.fogColor = _fogGrad.Evaluate(_currentTimeGrad);
            _directionalLight.color = _lightGrad.Evaluate(_currentTimeGrad);
            _skyBoxMaterial.SetColor("_Tint",_skyBoxGrad.Evaluate(_currentTimeGrad));
            _waterMaterial.SetColor("_SurfaceColor",_waterGrad.Evaluate(_currentTimeGrad));
            _waterMaterial.SetColor("_FoamColor", _foamGrad.Evaluate(_currentTimeGrad));
        }

        void rotateSkybox()
        {
            float currentRot = _skyBoxMaterial.GetFloat("_Rotation");
            float newRot = currentRot + _rotationSpeed * Time.deltaTime;
            newRot = Mathf.Repeat(newRot, 360f);
            _skyBoxMaterial.SetFloat("_Rotation", newRot);
        }
    }
}
