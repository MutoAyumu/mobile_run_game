using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class TestTouchPosition : MonoBehaviour
{
    GameInputs _inputs;
    [SerializeField] GameObject _go;

    private void Awake()
    {
        _inputs = new GameInputs();
    }

    private void OnEnable()
    {
        _inputs.Player.Touch.performed += OnTouch;
        _inputs.Enable();
    }

    private void OnTouch(InputAction.CallbackContext obj)
    {
        var state = obj.ReadValue<TouchState>();

        if (state.isInProgress)
        {
            var pos = state.position;

            if (pos.x >= 0 && pos.x < Screen.width && pos.y >= 0 && pos.y < Screen.height)
                _go.transform.position = pos;
        }
    }

    private void OnDisable()
    {
        _inputs.Player.Touch.performed -= OnTouch;
        _inputs.Disable();
    }
}
