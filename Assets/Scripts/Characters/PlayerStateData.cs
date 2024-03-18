using UnityEngine;

namespace QC.Characters
{
    public class PlayerStateData
    {
        public float SpeedModifier = 1f;
        public Vector2 MovementInput;
        private Vector3 _currentTargetRotation;
        private Vector3 _timeToReachTargetRotation;
        private Vector3 _dampedTargetRotationCurrentVelocity;
        private Vector3 _dampedTargetRotationPassedTime;

        public ref Vector3 CurrentTargetRotation
        {
            get
            {
                return ref _currentTargetRotation;
            }
        }

        public ref Vector3 TimeToReachTargetRotation
        {
            get
            {
                return ref _timeToReachTargetRotation;
            }
        }

        public ref Vector3 DampedTargetRotationCurrentVelocity
        {
            get
            {
                return ref _dampedTargetRotationCurrentVelocity;
            }
        }

        public ref Vector3 DampedTargetRotationPassedTime
        {
            get
            {
                return ref _dampedTargetRotationPassedTime;
            }
        }



    }

}
