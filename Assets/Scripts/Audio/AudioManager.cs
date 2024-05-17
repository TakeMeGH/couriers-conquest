using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;



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

    }

    public void AudioPlayOneShot(EventReference sound, Vector3 worldPos){
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(SFXVolume);
    }

    public void setMusicVolume()
    {

    }

}
