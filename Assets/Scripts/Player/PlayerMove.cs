using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1;
    [SerializeField] float _jumpPower = 1;

    float _currentSpeed;
    Vector2 _dir;
    Rigidbody _rb;

    public void Init(PlayerController player)
    {
        _rb = player.Rigidbody;
        player.Input.MoveVector.Subscribe(x => SetDirection(x)).AddTo(this);
        player.Input.JumpSub.Subscribe(_ => OnJump()).AddTo(this);
        player.OnUpdateSub.Subscribe(_ => OnUpdate()).AddTo(this);
        _currentSpeed = _moveSpeed;
    }

    void OnUpdate()
    {
        OnMove();
    }
    void OnMove()
    {
        if (_dir != Vector2.zero)
        {
            var vel = new Vector3(_dir.x, 0, 0).normalized * _currentSpeed;
            vel.y = _rb.velocity.y;
            _rb.velocity = vel;
        }
    }
    void OnJump()
    {
        var vel = _rb.velocity;
        vel.y = 0;
        _rb.velocity = vel;
        _rb.AddForce(Vector3.up * _jumpPower, ForceMode.VelocityChange);
    }
    void SetDirection(Vector2 vec)
    {
        _dir = new Vector2(vec.x, vec.y);
    }
}
