using UnityEngine;
using UnityEngine.InputSystem;

namespace Example.Input
{
    public class ExampleInputReader : MonoBehaviour
    {
        [SerializeField] InputReader inputReader;

        private void OnEnable()
        {
            inputReader.MoveEvent += OnMove;
            inputReader.JumpEvent += OnJump;
            inputReader.StartedRunning += OnRun;
            inputReader.StoppedRunning += OnRunStoped;
        }

        private void OnDisable()
        {
            inputReader.MoveEvent -= OnMove;
            inputReader.JumpEvent -= OnJump;
            inputReader.StartedRunning -= OnRun;
            inputReader.StoppedRunning -= OnRunStoped;
        }

        private void OnMove(Vector2 movement)
        {

            Debug.Log("MOVE " + movement);
        }

        private void OnJump()
        {

            Debug.Log("JUMP");
        }

        private void OnRun()
        {
            Debug.Log("RUN");
        }

        private void OnRunStoped()
        {
            Debug.Log("RUN Stoped");
        }


    }

}
