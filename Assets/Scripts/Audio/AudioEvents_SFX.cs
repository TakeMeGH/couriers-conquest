using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class FMOD_AudioEvents : MonoBehaviour
{
    [field: Header("UI Sounds")]
    [field: SerializeField] public EventReference Button_Hover {get;private set;}
    [field: SerializeField] public EventReference Button_Click {get;private set;}
    
    
    
    [field: Header("Character")]
    [field: SerializeField] public EventReference Player_Walk {get;private set;}
    public static FMOD_AudioEvents instance {get; private set;}

    private void Awake() {
         if (instance != null){
            Debug.LogError("Somehow there's more than one FMOD_AudioManager in the scene");
        }    
        instance = this;
    }
}
