using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have no arguments (Example: Exit game event)
/// </summary>

namespace CC.Events
{
    [CreateAssetMenu(menuName = "Game/Events/Float Event Channel")]
    public class FloatEventChannelSO : DescriptionBaseSO
    {
        public UnityAction<float> OnEventRaised;

        public void RaiseEvent(float value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);

        }
    }

}


