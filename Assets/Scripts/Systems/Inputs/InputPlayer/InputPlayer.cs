using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

public class InputPlayer : MonoBehaviour
{
    #region 変数
    ReactiveProperty<Vector2> _moveVector = new ReactiveProperty<Vector2>();
    private PlayerInput _input;
    #endregion

    #region プロパティ
    public IReadOnlyReactiveProperty<Vector2> MoveVector => _moveVector;
    #endregion

    #region UnityEvent
    private void Awake()
    {
        TryGetComponent(out _input);
    }

    private void OnEnable()
    {
        InputSystem.EnableDevice(GravitySensor.current);

        _input.actions["Move"].started += OnMove;
        _input.actions["Move"].performed += OnMove;
        _input.actions["Move"].canceled += OnMoveStop;
        _input.actions["Gyro"].started += OnMove;
        _input.actions["Gyro"].performed += OnMove;
    }

    private void OnDisable()
    {
        InputSystem.DisableDevice(GravitySensor.current);

        _input.actions["Move"].started -= OnMove;
        _input.actions["Move"].performed -= OnMove;
        _input.actions["Move"].canceled -= OnMoveStop;
        _input.actions["Gyro"].started -= OnMove;
        _input.actions["Gyro"].performed -= OnMove;
    }
    #endregion

    #region InputSystemEvent
    void OnMove(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<Vector2>();
        Debug.Log("OnMove :" + v);
        _moveVector.Value = v;
    }
    void OnMoveStop(InputAction.CallbackContext context)
    {
        Debug.Log("OnMoveStop :" + Vector2.zero);
        _moveVector.Value = Vector2.zero;
    }
    #endregion
}
