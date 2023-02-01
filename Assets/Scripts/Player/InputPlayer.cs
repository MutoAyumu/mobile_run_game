using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using System;

public class InputPlayer : MonoBehaviour
{
    #region 変数
    GameInputs _inputs;
    GravitySensor _gravitySensor;
    ReactiveProperty<Vector2> _moveVector = new ReactiveProperty<Vector2>();
    Subject<Unit> _jumpSub = new Subject<Unit>();

    [SerializeField] float _deadZone = 0.3f;
    [SerializeField, Range(0f, 1f)] float _clampRotateValue = 0.6f;
    #endregion

    #region プロパティ
    public IReadOnlyReactiveProperty<Vector2> MoveVector => _moveVector;
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
        StartCoroutine(EnableSensorAsync());

        _inputs.Player.Jump.performed += OnJump;

        _inputs.Enable();
    }

    void Disable()
    {
        if (_gravitySensor != null && _gravitySensor.enabled)
        {
            InputSystem.DisableDevice(GravitySensor.current);
            Debug.Log($"DisableDevice GravitySensor");
        }

        _inputs.Player.Jump.performed -= OnJump;

        _inputs.Disable();
    }
    void OnUpdate()
    {
        if (_gravitySensor != null)
        {
            var gravity = _gravitySensor.gravity.ReadValue();
            var anyInput = _inputs.Player.Move.ReadValue<Vector2>();

            if (Mathf.Abs(gravity.x) < _deadZone)
            {
                gravity = Vector2.zero;
            }

            if (Mathf.Abs(gravity.x) >= _clampRotateValue)
            {
                gravity = Vector2.one;
            }

            var vec = gravity;

            if (Mathf.Abs(anyInput.x) > Mathf.Abs(gravity.x))
            {
                vec = anyInput;
            }

            InputMove(vec);
        }
        else
        {
            var vec = _inputs.Player.Move.ReadValue<Vector2>();
            InputMove(vec);
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

    #region Event
    void InputMove(Vector2 vec)
    {
        _moveVector.Value = vec;
    }
    void OnJump(InputAction.CallbackContext context)
    {
        _jumpSub.OnNext(Unit.Default);
    }
    #endregion
}
