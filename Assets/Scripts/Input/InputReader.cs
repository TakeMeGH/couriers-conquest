using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using CC.Events;

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
    public event UnityAction BlockPerformed = delegate { };
    public event UnityAction BlockCanceled = delegate { };
    public event UnityAction WalkToggleStarted = delegate { };
    public event UnityAction DropClimbingPerformed = delegate { };
    public event UnityAction<float> ScrollInteracionPerformed = delegate { };
    public event UnityAction PouchPerformed = delegate { };
    public event UnityAction PausePerformed = delegate { };


    public bool IsBlocking { get; private set; }
    public bool IsAttacking { get; private set; }


    public event UnityAction OpenInventoryEvent = delegate { };
    public event UnityAction CloseInventoryEvent = delegate { };
    public event UnityAction DropItemPerformed = delegate { };
    [SerializeField] VoidEventChannelSO _enableCameraInputEvent;

    [SerializeField] VoidEventChannelSO _disableCameraInputEvent;



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

    public void OnBlock(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                BlockPerformed.Invoke();
                IsBlocking = true;
                break;
            case InputActionPhase.Canceled:
                BlockCanceled.Invoke();
                IsBlocking = false;
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
        _enableCameraInputEvent.RaiseEvent();
        _playerInput.InventoryUI.Disable();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void EnableInventoryUIInput()
    {
        _playerInput.Gameplay.Disable();
        _disableCameraInputEvent.RaiseEvent();
        _playerInput.InventoryUI.Enable();
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void EnableScrollInteracionInput()
    {
        DisableSpecificAction("Gameplay", "Zoom");
        EnableSpecificAction("Gameplay", "ScrolIInteraction");
    }

    public void DisableScrollInteracionInput()
    {
        EnableSpecificAction("Gameplay", "Zoom");
        EnableSpecificAction("Gameplay", "ScrolIInteraction");
    }


    public void OnWalkToggle(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            WalkToggleStarted.Invoke();
    }

    public void OnScrolIInteraction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ScrollInteracionPerformed.Invoke(context.ReadValue<float>());
        }
    }

    public void OnDropClimbing(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            DropClimbingPerformed.Invoke();
        }
    }


    public void DisableSpecificAction(string _actionMapName, string _actionName)
    {
        InputActionMap _actionMap = _playerInput.asset.FindActionMap(_actionMapName);

        if (_actionMap == null)
        {
            Debug.LogError("Action Map not found");
        }

        InputAction _action = _actionMap.FindAction(_actionName);

        if (_action == null)
        {
            Debug.LogError("Input Action not found");
        }
        _action.Disable();
    }

    public void EnableSpecificAction(string _actionMapName, string _actionName)
    {
        InputActionMap _actionMap = _playerInput.asset.FindActionMap(_actionMapName);

        if (_actionMap == null)
        {
            Debug.LogError("Action Map not found");
        }

        InputAction _action = _actionMap.FindAction(_actionName);

        if (_action == null)
        {
            Debug.LogError("Input Action not found");
        }
        _action.Enable();
    }

    public void OnPouch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            PouchPerformed.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            PausePerformed.Invoke();
    }
}
