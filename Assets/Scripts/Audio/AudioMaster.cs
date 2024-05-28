using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FMODUnity;
using FMOD;
using FMOD.Studio;

namespace CC
{
    public class AudioMaster : MonoBehaviour
    {

        [Header("Volume")]
        [Range(0, 1)]

        public float masterVolume = 1;
        [Range(0,1)]

        public float musicVolume = 1;
        [Range(0, 1)]

        public float SFXVolume = 1;


        private Bus masterBus;
        private Bus musicBus;
        private Bus sfxBus;



        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
