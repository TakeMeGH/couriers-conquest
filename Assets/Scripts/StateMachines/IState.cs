namespace CC.StateMachine
{
    public interface IState
    {
        public void Enter();
        public void Update();
        public void PhysicsUpdate();
        public void Exit();
        public void OnAnimationEnterEvent();
        public void OnAnimationExitEvent();
        public void OnAnimationTransitionEvent();


    }

}
