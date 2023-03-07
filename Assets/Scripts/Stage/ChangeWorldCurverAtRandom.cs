using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWorldCurverAtRandom : MonoBehaviour
{
    [SerializeField] WorldCurver _curver;
    [SerializeField, Range(0, 0.03f)] float _speed = 5f;
    [SerializeField] float _curvatureRange = 0.025f;
    [SerializeField] float _interval = 15f;
    float _value;
    bool _isAddition;
    Timer _timer = new Timer();

    private void Awake()
    {
        _timer.Setup(_interval);
    }

    private void Update()
    {
        var v = Mathf.Clamp(_value, -_curvatureRange, _curvatureRange);

        if (v >= _curvatureRange)
        {
            if (!_timer.RunTimer()) return;

            _isAddition = false;
        }
        else if (v <= -_curvatureRange)
        {
            if (!_timer.RunTimer()) return;

            _isAddition = true;
        }

        _curver.SetCurve(v);

        if (_isAddition)
            _value += Time.deltaTime * _speed;
        else
            _value -= Time.deltaTime * _speed;
    }
}
