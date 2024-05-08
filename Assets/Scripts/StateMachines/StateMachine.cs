
using UnityEngine;

namespace CC.StateMachine
{
    public abstract class StateMachine : MonoBehaviour
    {
        private IState currentState;
        bool _onSwitchState = false;

        public void SwitchState(IState newState)
        {
            _onSwitchState = true;
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
            _onSwitchState = false;
        }

        private void Update()
        {
            if (!_onSwitchState) currentState?.Update();
        }

        private void FixedUpdate()
        {
            if (!_onSwitchState) currentState?.PhysicsUpdate();

        }

        public System.Type GetCurrentStateType()
        {
            return currentState.GetType();
        }

        public IState GetCurrentState()
        {
            return currentState;
        }
    }
}
