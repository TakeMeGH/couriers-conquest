using UnityEngine;

namespace CC.Characters
{
    public class PlayerStateData
    {
        public Vector2 MovementInput;
        public float MovementSpeedModifier = 1f;
        public bool ShouldSprint;
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

        [field: SerializeField] public Vector3 TargetRotationReachTime { get; set; }


    }

}
