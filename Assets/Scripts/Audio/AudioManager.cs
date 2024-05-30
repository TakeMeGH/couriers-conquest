using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using CC;



public class AudioManager : MonoBehaviour
{

    public static AudioManager instance {
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

    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;

    private void Awake() {
        if (instance != null){
            Debug.LogError("Somehow there's more than one AudioManager in the scene");
        }    
        instance = this;

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/Sound Effects");

        // masterBus.getVolume(out masterVolume);
        // musicBus.getVolume(out musicVolume);
        // sfxBus.getVolume(out SFXVolume);

        masterVolume = settingValue.MasterVolumeValue;
        musicVolume = settingValue.MusicVolumeValue;
        SFXVolume = settingValue.SFXVolumeValue;
        
        
        // masterBus.setVolume(settingValue.MasterVolumeValue);
        // musicBus.setVolume(settingValue.MusicVolumeValue);
        // sfxBus.setVolume(settingValue.SFXVolumeValue);        
    }

    
    public void AudioPlayOneShot(EventReference sound, Vector3 worldPos){
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    
    public void saveConfirmedVolume(){
        masterBus.getVolume(out settingValue.MasterVolumeValue);
        musicBus.getVolume(out settingValue.MusicVolumeValue);
        sfxBus.getVolume(out settingValue.SFXVolumeValue);
    }
    
    public void discardConfirmedVolume(){


        masterVolume = settingValue.MasterVolumeValue;
        musicVolume = settingValue.MusicVolumeValue;
        SFXVolume = settingValue.SFXVolumeValue;

        
        masterBus.setVolume(settingValue.MasterVolumeValue);
        musicBus.setVolume(settingValue.MusicVolumeValue);
        sfxBus.setVolume(settingValue.SFXVolumeValue);


    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(SFXVolume);
    }


}
