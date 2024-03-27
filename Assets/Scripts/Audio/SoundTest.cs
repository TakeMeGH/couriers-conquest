using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    public void Button_Click(){AudioManager.instance.AudioPlayOneShot(FMOD_AudioEvents.instance.Button_Click, this.transform.position);}
    public void Button_Hover(){AudioManager.instance.AudioPlayOneShot(FMOD_AudioEvents.instance.Button_Hover, this.transform.position);}
}
