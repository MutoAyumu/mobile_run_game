using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMove
{
    float _currentSpeed;
    float _runSpeed;
    Vector2 _dir;
    Rigidbody _rb;

    public TestPlayerMove(float speed, Rigidbody rb)
    {
        _runSpeed = speed;
        _currentSpeed = _runSpeed;
        _rb = rb;
    }
    public void SetDirection(Vector2 vec)
    {
        _dir = vec;
    }
    public void OnMove()
    {
        if (_dir != Vector2.zero)
        {
            _rb.AddForce(new Vector3(_dir.x, -_rb.velocity.y, _dir.y).normalized * _currentSpeed, ForceMode.Acceleration);
        }
    }
}
