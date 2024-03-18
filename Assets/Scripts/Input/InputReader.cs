using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : DescriptionBaseSO, GameInput.IGameplayActions
{
    // Assign delegate{} to events to initialise them with an empty delegate
    // so we can skip the null check when we use them

    // Gameplay
    public event UnityAction JumpEvent = delegate { };
    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction StartedRunning = delegate { };
    public event UnityAction StoppedRunning = delegate { };

    GameInput _playerInput;

    private void OnEnable() {
        if(_playerInput == null){
            _playerInput = new GameInput();
            _playerInput.Gameplay.SetCallbacks(this);
        }
        _playerInput.Gameplay.Enable();

    }

    private void OnDisable() {
        if(_playerInput != null) _playerInput.Gameplay.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            JumpEvent.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                StartedRunning.Invoke();
                break;
            case InputActionPhase.Canceled:
                StoppedRunning.Invoke();
                break;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {}

    public void OnZoom(InputAction.CallbackContext context)
    {}

    public void OnDash(InputAction.CallbackContext context)
    {
    }
}
