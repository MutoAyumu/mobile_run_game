using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using System;
using UnityEngine.InputSystem.LowLevel;

public class InputSystemManager
{
    #region 変数
    GameInputs _inputs;
    ReactiveProperty<Vector2> _moveVector = new ReactiveProperty<Vector2>();
    ReactiveProperty<TouchState> _touchState = new ReactiveProperty<TouchState>();
    ReactiveProperty<Vector2> _editorTouchPoint = new ReactiveProperty<Vector2>();
    Subject<Unit> _editorTouchButton = new Subject<Unit>();
    Subject<Unit> _jumpSub = new Subject<Unit>();
    Subject<Unit> _actionSub = new Subject<Unit>();
    #endregion

    #region プロパティ
    public static InputSystemManager Instance => new InputSystemManager();
    public IReadOnlyReactiveProperty<Vector2> MoveVector => _moveVector;
    public IReadOnlyReactiveProperty<TouchState> TouchState => _touchState;
    public IReadOnlyReactiveProperty<Vector2> EditorTouchPoint => _editorTouchPoint;
    public IObservable<Unit> EditorTouchButton => _editorTouchButton;
    public IObservable<Unit> JumpSub => _jumpSub;
    public IObservable<Unit> ActionSub => _actionSub;
    #endregion

    private InputSystemManager()
    {
        _inputs = new GameInputs();
        _inputs.Player.Jump.performed += OnInputJump;
        _inputs.Player.Move.performed += OnInputMove;
        _inputs.Player.Move.canceled += OnInputMove;
        _inputs.Player.Action.performed += OnInputAction;

#if UNITY_EDITOR
        if(UnityEditor.EditorApplication.isRemoteConnected)
        {
            _inputs.Player.Touch.performed += OnInputTouch;
        }
        else
        {
            _inputs.Player.EditorTouchButton.started += OnInputEditorTouchButton;
            _inputs.Player.EditorTouchButton.canceled += OnInputEditorTouchButton;
            _inputs.Player.EditorTouchPoint.performed += OnInputEditorTouchPoint;
        }
#endif

#if UNITY_ANDROID
        _inputs.Player.Touch.performed += OnInputTouch;
#endif

        _inputs.Enable();
    }

#region InputEvent
    void OnInputMove(InputAction.CallbackContext context)
    {
        _moveVector.Value = context.ReadValue<Vector2>();
    }
    void OnInputJump(InputAction.CallbackContext context)
    {
        _jumpSub.OnNext(Unit.Default);
    }
    void OnInputTouch(InputAction.CallbackContext context)
    {
        _touchState.Value = context.ReadValue<TouchState>();
    }
    void OnInputEditorTouchButton(InputAction.CallbackContext context)
    {
        _editorTouchButton.OnNext(Unit.Default);
    }
    void OnInputEditorTouchPoint(InputAction.CallbackContext context)
    {
        _editorTouchPoint.Value = context.ReadValue<Vector2>();
    }
    void OnInputAction(InputAction.CallbackContext context)
    {
        _actionSub.OnNext(Unit.Default);
    }
#endregion
}
