using UnityEngine;

namespace CC.Characters
{
    public class PlayerStateData
    {
        public Vector2 MovementInput;
        public float MovementSpeedModifier = 1f;
        public float MovementOnSlopesSpeedModifier { get; set; } = 1f;
        public float MovementDecelerationForce { get; set; } = 1f;
        public float MaxDropTime { get; set; } = 2f;
        public float CurrentDropTime { get; set;} = 0;
        public bool ShouldSprint;
        public bool ShouldWalk;

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
        public Vector3 CurrentJumpForce { get; set; }
        [field: SerializeField] public Vector3 TargetRotationReachTime { get; set; }


    }

}
