
using UnityEngine;

namespace CC.StateMachine
{
    public abstract class StateMachine : MonoBehaviour
    {
        private IState currentState;

        public void SwitchState(IState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }

        private void Update()
        {
            currentState?.Update();
        }

        private void FixedUpdate()
        {
            currentState?.PhysicsUpdate();

        }

        public System.Type GetCurrentStateType()
        {
            return currentState.GetType();
        }
    }
}
