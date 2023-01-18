using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMove : MonoBehaviour
{
    float _currentSpeed;
    float _runSpeed;
    Vector2 _dir;
    Rigidbody _rb;

    public void Init(TestPlayer player)
    {
        _rb = player.Rb;
        _currentSpeed = player.Speed;
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
