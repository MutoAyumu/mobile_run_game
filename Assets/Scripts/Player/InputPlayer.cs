using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using System;
using UnityEngine.InputSystem.LowLevel;

public class InputPlayer : MonoBehaviour
{
    #region 変数
    GameInputs _inputs;
    ReactiveProperty<Vector2> _moveVector = new ReactiveProperty<Vector2>();
    ReactiveProperty<TouchState> _touchState = new ReactiveProperty<TouchState>();
    Subject<Unit> _jumpSub = new Subject<Unit>();
    #endregion

    #region プロパティ
    public IReadOnlyReactiveProperty<Vector2> MoveVector => _moveVector;
    public IReadOnlyReactiveProperty<TouchState> TouchState => _touchState;
    public IObservable<Unit> JumpSub => _jumpSub;
    #endregion

    #region UnityEvent

    public void Init(PlayerController player)
    {
        player.OnUpdateSub.Subscribe(_ => OnUpdate()).AddTo(this);
        player.OnEnableSub.Subscribe(_ => Enable()).AddTo(this);
        player.OnDisableSub.Subscribe(_ => Disable()).AddTo(this);
        _inputs = new GameInputs();
    }
    void Enable()
    {
        _inputs.Player.Jump.performed += OnInputJump;
        _inputs.Player.Move.performed += OnInputMove;
        _inputs.Player.Move.canceled += OnInputMove;
        _inputs.Player.Touch.performed += OnInputTouch;

        _inputs.Enable();
    }

    void Disable()
    {
        _inputs.Player.Jump.performed -= OnInputJump;
        _inputs.Player.Move.performed -= OnInputMove;
        _inputs.Player.Move.canceled -= OnInputMove;
        _inputs.Player.Touch.performed -= OnInputTouch;

        _inputs.Disable();
    }
    void OnUpdate() { }
    #endregion

    #region InputEvent
    void OnInputMove(InputAction.CallbackContext context)
    {
        _moveVector.Value = context.ReadValue<Vector2>();
    }
    void OnInputJump(InputAction.CallbackContext context)
    {
        _jumpSub.OnNext(Unit.Default);
    }
    private void OnInputTouch(InputAction.CallbackContext context)
    {
        _touchState.Value = context.ReadValue<TouchState>();
    }
    #endregion
}
