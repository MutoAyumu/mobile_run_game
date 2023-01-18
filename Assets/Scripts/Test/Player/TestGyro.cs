using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestGyro : MonoBehaviour
{
    float? _valueX;
    GUIStyle _style = new GUIStyle();

    private void Start()
    {
        _style.fontSize = 150;

    }

    private void Update()
    {
        var gravitySensor = GravitySensor.current;
        if (gravitySensor != null)
        {
            InputSystem.EnableDevice(GravitySensor.current);
            _valueX = gravitySensor.gravity.ReadValue().x;
        }
    }
    void OnGUI()
    {
        var t = "0";

        if (_valueX != null)
        {
            t = _valueX.Value.ToString("0.00");
        }

        var rect = new Rect(0, 0, 2400, 1080);
        GUI.Label(rect, t, _style);
    }
}