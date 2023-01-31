using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using System;

public class TestInputPlayer : MonoBehaviour
{
    #region 変数
    ReactiveProperty<Vector2> _moveVector = new ReactiveProperty<Vector2>();
    PlayerInput _input;
    GravitySensor _gravitySensor;
    [SerializeField] float _deadZone = 0.3f;
    [SerializeField, Range(0f, 1f)] float _clampRotateValue = 0.6f;
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
            var gravity = _gravitySensor.gravity.ReadValue();
            var anyInput = _input.actions["Move"].ReadValue<Vector2>();

            if (Math.Abs(gravity.x) < _deadZone)
            {
                gravity = Vector2.zero;
            }

            if(Math.Abs(gravity.x) >= _clampRotateValue)
            {
                gravity = Vector2.one;
            }

            var vec = gravity;

            if(Mathf.Abs(anyInput.x) > MathF.Abs(gravity.x))
            {
                vec = anyInput;
            }

            OnMove(vec);
        }
        else
        {
            var vec = _input.actions["Move"].ReadValue<Vector2>();
            OnMove(vec);
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
    void OnMove(Vector2 vec)
    {
        _moveVector.Value = vec;
    }
    #endregion
}
