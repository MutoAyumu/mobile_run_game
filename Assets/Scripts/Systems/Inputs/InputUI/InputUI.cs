using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputUI : MonoBehaviour, InputActionData.IUIActions
{
    private InputActionData _input;

    #region UnityEvent
    private void Awake()
    {
        _input = new InputActionData();
        _input.UI.SetCallbacks(this);
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void OnDestroy()
    {
        _input.Dispose();
    }
    #endregion

    #region InputSystemEvent
    public void OnTouch_0(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
