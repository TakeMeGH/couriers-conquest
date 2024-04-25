using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have no arguments (Example: Exit game event)
/// </summary>

namespace CC.Events
{
    [CreateAssetMenu(menuName = "Game/Events/Character Event Channel")]
    public class CharacterEventChannelSO : DescriptionBaseSO
    {
        public UnityAction<GameObject> OnEventRaised;

        public void RaiseEvent(GameObject _character)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(_character);
        }
    }

}


