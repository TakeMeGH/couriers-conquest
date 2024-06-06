using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using CC;
using System.Collections;
using CC.Event;



public class AudioManager : MonoBehaviour
{

    public static AudioManager instance
    {
        get;
        private set;
    }

    [Header("Volume")]

    [Range(0, 1)]
    public float masterVolume = 1;

    [Range(0, 1)]
    public float musicVolume = 1;

    [Range(0, 1)]
    public float SFXVolume = 1;

    [SerializeField]
    private SettingsValueHolder settingValue;
    [Header("SFX UI")]
    [SerializeField] public EventReference ConfirmUI;
    [SerializeField] public EventReference BackUI;
    [SerializeField] public EventReference DialogueUI;

    [Header("SFX Player")]
    public EventReference DashPlayer;
    public EventReference WalkPlayer;
    public EventReference SwordSwing;
    public EventReference SwordHit;
    public EventReference ShieldHit;

    [Header("SFX NPC")]
    public EventReference BlacksmithHit;
    public EventReference GoblinNotice;
    public EventReference BanditNotice;

    [Header("BGM")]
    [SerializeField] public EventReference MainMenuBGM;
    [SerializeField] public EventReference CityBGM;
    [SerializeField] public EventReference ForestBGM;
    [SerializeField] public EventReference VillageBGM;
    [SerializeField] public EventReference DeadBGM;
    [Header("Event")]
    [SerializeField] SenderDataEventChannelSO _OnLoadFinished;


    EventInstance _BGMEventInstance;
    EventReference _currentBGMReference;


    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Somehow there's more than one AudioManager in the scene");
        }
        instance = this;

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/Sound Effects");

        masterVolume = settingValue.MasterVolumeValue;
        musicVolume = settingValue.MusicVolumeValue;
        SFXVolume = settingValue.SFXVolumeValue;

        _OnLoadFinished.OnEventRaised.AddListener(StopBGM);
    }

    public bool IsPlayingBGM()
    {
        return _BGMEventInstance.isValid();
    }

    public void InitializeBGM(EventReference BGMReference)
    {
        if (_currentBGMReference.Path == BGMReference.Path)
        {
            return;
        }
        StartCoroutine(FadeOutAndChangeBGM(BGMReference));
    }

    public void StopBGM(Component component, object sender)
    {
        if (!_BGMEventInstance.isValid()) return;
        _BGMEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _BGMEventInstance.release();

    }
    private IEnumerator FadeOutAndChangeBGM(EventReference newBGMReference)
    {
        if (_BGMEventInstance.isValid())
        {
            _BGMEventInstance.getVolume(out float currentVolume);
            float fadeDuration = 2.0f;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float newVolume = Mathf.Lerp(currentVolume, 0f, elapsedTime / fadeDuration);
                _BGMEventInstance.setVolume(newVolume);
                yield return null;
            }

            _BGMEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            _BGMEventInstance.release();
        }

        _BGMEventInstance = RuntimeManager.CreateInstance(newBGMReference);
        _BGMEventInstance.start();
        _currentBGMReference = newBGMReference;
    }
    public void AudioPlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }


    public void saveConfirmedVolume()
    {
        masterBus.getVolume(out settingValue.MasterVolumeValue);
        musicBus.getVolume(out settingValue.MusicVolumeValue);
        sfxBus.getVolume(out settingValue.SFXVolumeValue);
        Debug.Log(settingValue.MasterVolumeValue + " DEBUG");
    }

    public void discardConfirmedVolume()
    {
        masterVolume = settingValue.MasterVolumeValue;
        musicVolume = settingValue.MusicVolumeValue;
        SFXVolume = settingValue.SFXVolumeValue;

        masterBus.setVolume(settingValue.MasterVolumeValue);
        musicBus.setVolume(settingValue.MusicVolumeValue);
        sfxBus.setVolume(settingValue.SFXVolumeValue);


    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            InitializeBGM(AudioManager.instance.MainMenuBGM);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            InitializeBGM(AudioManager.instance.CityBGM);

        }
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(SFXVolume);
    }

    private void OnDestroy()
    {
        StopBGM(null, null);
    }

}
