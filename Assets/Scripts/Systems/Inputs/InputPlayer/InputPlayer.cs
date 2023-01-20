using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using System;

public class InputPlayer : MonoBehaviour
{
    #region 変数
    ReactiveProperty<Vector2> _moveVector = new ReactiveProperty<Vector2>();
    PlayerInput _input;
    GravitySensor _gravitySensor;
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
        StartCoroutine(EnableSensorAsync());

        _input.actions["Move"].started += OnMove;
        _input.actions["Move"].performed += OnMove;
        _input.actions["Move"].canceled += OnMoveStop;
    }

    private void OnDisable()
    {
        if (_gravitySensor != null && _gravitySensor.enabled)
        {
            InputSystem.DisableDevice(GravitySensor.current);
            Debug.Log($"DisableDevice GravitySensor");
        }

        _input.actions["Move"].started -= OnMove;
        _input.actions["Move"].performed -= OnMove;
        _input.actions["Move"].canceled -= OnMoveStop;
    }
    private void Update()
    {
        //if (_gravitySensor != null)
        //{
        //    var v = _gravitySensor.gravity.ReadValue();
        //    _moveVector.Value = new Vector2(v.x, 0);
        //}
    }
    #endregion

    IEnumerator EnableSensorAsync()
    {
        Debug.Log($"Start EnableSensorAsync");
        
        while (true)
        {
            yield return null;

            if (_gravitySensor == null)
            {
                _gravitySensor = GravitySensor.current;
            }
            else
            {
                if (!_gravitySensor.enabled)
                {
                    InputSystem.EnableDevice(GravitySensor.current);
                    Debug.Log($"EnableDevice GravitySensor");
                }

                break;
            }
        }

        Debug.Log($"Complete EnableSensorAsync");
    }

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
