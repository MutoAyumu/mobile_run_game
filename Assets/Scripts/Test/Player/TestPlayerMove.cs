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
        _dir = new Vector2(vec.x, vec.y);
    }
    public void OnMove()
    {
        if (_dir != Vector2.zero)
        {
            _rb.velocity += new Vector3(_dir.x, _dir.y, 0).normalized * _currentSpeed;
            //_rb.AddForce(new Vector3(_dir.x, _dir.y , 0).normalized * _currentSpeed, ForceMode.Acceleration);
        }
    }
}
