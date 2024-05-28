using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CC
{
    public class AudioSlider : MonoBehaviour
    {


        private enum VolumeType
        {
            MASTER, MUSIC, SFX
        }

        [Header("Volume Type")]
        [SerializeField] private VolumeType volumeType;

        private Slider volumeSlider;



        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            switch (volumeType)
            {
                case VolumeType.MASTER:
                    volumeSlider.value = AudioManager.instance.masterVolume;
                    break;
                case VolumeType.MUSIC:
                    volumeSlider.value = AudioManager.instance.musicVolume;
                    break;
                case VolumeType.SFX:
                    volumeSlider.value = AudioManager.instance.SFXVolume;
                    break;
                default:
                    Debug.LogWarning("ERROR WARNING : Not supported Volume type");
                    break;
            }
        }

        private void Awake()
        {
            volumeSlider = this.GetComponentInChildren<Slider>();
        }

        public void OnSliderValueChange()
        {
            switch (volumeType)
            {
                case VolumeType.MASTER:
                    AudioManager.instance.masterVolume = volumeSlider.value;
                    break;
                case VolumeType.MUSIC:
                    AudioManager.instance.musicVolume = volumeSlider.value;
                    break;
                case VolumeType.SFX:
                    AudioManager.instance.SFXVolume = volumeSlider.value;
                    break;
                default:
                    Debug.LogWarning("ERROR WARNING : Not supported Volume type");
                    break;
            }
        }
    }
}
