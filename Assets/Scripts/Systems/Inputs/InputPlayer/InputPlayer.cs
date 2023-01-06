using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputPlayer : MonoBehaviour, InputActionData.IPlayerActions
{
    private InputActionData _input;
    #region 変数
    Vector2 _moveVector;
    #endregion

    #region プロパティ
    #endregion
    public Vector2 MoveVector => _moveVector;

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
        _moveVector = v;
    }
    public void OnMoveStop(InputAction.CallbackContext context)
    {
        Debug.Log("Move :" + Vector2.zero);
        _moveVector = Vector2.zero;
    }
    #endregion
}
