using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

public class InputPlayer : MonoBehaviour, InputActionData.IPlayerActions
{
    private InputActionData _input;
    #region 変数
    ReactiveProperty<Vector2> _moveVector = new ReactiveProperty<Vector2>();
    #endregion

    #region プロパティ
    #endregion
    public IReadOnlyReactiveProperty<Vector2> MoveVector => _moveVector;
    [SerializeField] TestGyro g;

    #region UnityEvent
    private void Awake()
    {
        _input = new InputActionData();
        _input.Player.SetCallbacks(this);
    }

    private void OnEnable()
    {
        _input.Player.Move.started += OnMove;
        _input.Player.Move.performed += OnMove;
        _input.Player.Move.canceled += OnMoveStop;
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Player.Move.started -= OnMove;
        _input.Player.Move.performed -= OnMove;
        _input.Player.Move.canceled -= OnMoveStop;
        _input.Disable();
    }

    private void OnDestroy()
    {
        _input.Dispose();
    }
    #endregion

    #region InputSystemEvent
    public void OnMove(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<Vector2>();
        Debug.Log("Move :" + v);
        _moveVector.Value = new Vector2(g.Value, 0);
    }
    public void OnMoveStop(InputAction.CallbackContext context)
    {
        Debug.Log("Move :" + Vector2.zero);
        _moveVector.Value = Vector2.zero;
    }
    #endregion
}
