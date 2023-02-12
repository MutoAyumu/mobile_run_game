using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using System;
using UnityEngine.InputSystem.LowLevel;

public class InputSystemManager
{
    #region 変数
    GameInputs _inputs;
    Vector2 _moveVector;
    TouchState _touchState;
    Subject<Unit> _jumpSub = new Subject<Unit>();
    Subject<Unit> _actionSub = new Subject<Unit>();
    #endregion

    #region プロパティ
    public static InputSystemManager Instance => new InputSystemManager();
    public Vector2 MoveVector => _moveVector;
    public TouchState TouchState => _touchState;
    public IObservable<Unit> JumpSub => _jumpSub;
    public IObservable<Unit> ActionSub => _actionSub;
    #endregion

    private InputSystemManager()
    {
        _inputs = new GameInputs();
        _inputs.Player.Jump.performed += OnInputJump;
        _inputs.Player.Move.performed += OnInputMove;
        _inputs.Player.Move.canceled += OnInputMove;
        _inputs.Player.Touch.performed += OnInputTouch;
        _inputs.Player.Action.performed += OnInputAction;

        _inputs.Enable();
    }

    #region InputEvent
    void OnInputMove(InputAction.CallbackContext context)
    {
        _moveVector = context.ReadValue<Vector2>();
    }
    void OnInputJump(InputAction.CallbackContext context)
    {
        _jumpSub.OnNext(Unit.Default);
    }
    void OnInputTouch(InputAction.CallbackContext context)
    {
        _touchState = context.ReadValue<TouchState>();
    }
    void OnInputAction(InputAction.CallbackContext context)
    {
        _actionSub.OnNext(Unit.Default);
    }
    #endregion
}
