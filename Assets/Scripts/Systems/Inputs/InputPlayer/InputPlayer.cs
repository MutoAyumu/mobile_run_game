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
    [SerializeField] float _test = 0.3f;
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
    }

    private void OnDisable()
    {
        if (_gravitySensor != null && _gravitySensor.enabled)
        {
            InputSystem.DisableDevice(GravitySensor.current);
            Debug.Log($"DisableDevice GravitySensor");
        }
    }
    private void Update()
    {
        if (_gravitySensor != null)
        {
            var gravityX = _gravitySensor.gravity.ReadValue().x;
            var anyInputX = _input.actions["Move"].ReadValue<Vector2>().x;

            if (Math.Abs(gravityX) < _test)
            {
                gravityX = 0;
            }

            var x = gravityX;

            if(Mathf.Abs(anyInputX) > MathF.Abs(gravityX))
            {
                x = anyInputX;
            }

            OnMove(x);
        }
        else
        {
            var x = _input.actions["Move"].ReadValue<Vector2>().x;
            OnMove(x);
        }
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
    void OnMove(float x)
    {
        _moveVector.Value = new Vector2(x, 0);
    }
    #endregion
}
