using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQ.StatesInteface
{
    public interface IState
    {
        public void Enter();
        public void Update();
        public void PhysicsUpdate();

        public void Exit();

    }

}
