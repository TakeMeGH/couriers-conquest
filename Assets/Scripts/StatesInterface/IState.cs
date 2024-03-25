using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.StatesInteface
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
