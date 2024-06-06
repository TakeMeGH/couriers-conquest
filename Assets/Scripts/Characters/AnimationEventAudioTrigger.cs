using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CC
{
    public class AnimationEventAudioTrigger : MonoBehaviour
    {
        public void PlayAudio(AudioEnum usedAudio)
        {
            switch (usedAudio)
            {
                case AudioEnum.Dash:
                    AudioManager.instance.AudioPlayOneShot(AudioManager.instance.DashPlayer, transform.position);
                    break;
                case AudioEnum.SwordSwing:
                    AudioManager.instance.AudioPlayOneShot(AudioManager.instance.SwordSwing, transform.position);
                    break;
                case AudioEnum.SwordHit:
                    AudioManager.instance.AudioPlayOneShot(AudioManager.instance.SwordHit, transform.position);
                    break;
                case AudioEnum.Walk:
                    AudioManager.instance.AudioPlayOneShot(AudioManager.instance.WalkPlayer, transform.position);
                    break;
                case AudioEnum.BlacksmithHit:
                    AudioManager.instance.AudioPlayOneShot(AudioManager.instance.BlacksmithHit, transform.position);
                    break;


            }
        }
    }
    public enum AudioEnum
    {
        Dash,
        SwordSwing,
        SwordHit,
        Walk,
        BlacksmithHit,
    }
}
