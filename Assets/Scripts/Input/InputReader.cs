using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : DescriptionBaseSO, GameInput.IGameplayActions, GameInput.IInventoryUIActions
{
    // Assign delegate{} to events to initialise them with an empty delegate
    // so we can skip the null check when we use them

    // Gameplay
    public event UnityAction JumpPerformed = delegate { };
    public event UnityAction DashPerformed = delegate { };
    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction<Vector2> MoveStarted = delegate { };
    public event UnityAction<Vector2> MovePerformed = delegate { };
    public event UnityAction MoveCanceled = delegate { };
    public event UnityAction StartedRunning = delegate { };
    public event UnityAction StoppedRunning = delegate { };
    public event UnityAction StartedSprinting = delegate { };
    public event UnityAction StopedSprinting = delegate { };
    public event UnityAction InteractPerformed = delegate { };
    public event UnityAction TargetEvent = delegate { };
    public event UnityAction CancelEvent = delegate { };
    public event UnityAction AttackPerformed = delegate { };
    public event UnityAction AttackCanceled = delegate { };
    public bool IsAttacking {get; private set;}


    public event UnityAction OpenInventoryEvent = delegate { };
    public event UnityAction CloseInventoryEvent = delegate { };
    public event UnityAction DropItemPerformed = delegate { };



    GameInput _playerInput;

    private void OnEnable()
    {
        if (_playerInput == null)
        {
            _playerInput = new GameInput();
            _playerInput.Gameplay.SetCallbacks(this);
            _playerInput.InventoryUI.SetCallbacks(this);
        }
        EnableGameplayInput();
    }


    private void OnDisable()
    {
        if (_playerInput != null) _playerInput.Gameplay.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            JumpPerformed.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent.Invoke(context.ReadValue<Vector2>());
        switch (context.phase)
        {
            case InputActionPhase.Canceled:
                MoveCanceled.Invoke();
                break;
            case InputActionPhase.Performed:
                MovePerformed.Invoke(context.ReadValue<Vector2>());
                break;
            case InputActionPhase.Started:
                MoveStarted.Invoke(context.ReadValue<Vector2>());
                break;
        }
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
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            DashPerformed.Invoke();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                StartedSprinting.Invoke();
                break;
            case InputActionPhase.Canceled:
                StopedSprinting.Invoke();
                break;
        }

    }


    public void OnLook(InputAction.CallbackContext context)
    { }

    public void OnZoom(InputAction.CallbackContext context)
    { }

    public void OnInteract(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                InteractPerformed.Invoke();
                break;
        }
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            TargetEvent.Invoke();

    }
    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            CancelEvent.Invoke();


    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                AttackPerformed.Invoke();
                IsAttacking = true;
                break;
            case InputActionPhase.Canceled:
                AttackCanceled.Invoke();
                IsAttacking = false;
                break;
        }

    }


    public void OnOpenInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            OpenInventoryEvent.Invoke();
    }


    public void OnCloseInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            CloseInventoryEvent.Invoke();
    }

    public void OnDropItem(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            DropItemPerformed.Invoke();
    }

    public void EnableGameplayInput()
    {
        _playerInput.Gameplay.Enable();
        _playerInput.InventoryUI.Disable();
    }

    public void EnableInventoryUIInput()
    {
        _playerInput.Gameplay.Disable();
        _playerInput.InventoryUI.Enable();
    }


}
