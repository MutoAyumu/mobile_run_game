using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float _moveSpeed;

    float _currentSpeed;
    Vector2 _dir;
    Rigidbody _rb;

    public void Init(PlayerController player)
    {
        _rb = player.Rigidbody;
        player.Input.MoveVector.Subscribe(x => SetDirection(x)).AddTo(this);
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
            _rb.velocity += new Vector3(_dir.x, _dir.y, 0).normalized * _currentSpeed;
            //_rb.AddForce(new Vector3(_dir.x, _dir.y , 0).normalized * _currentSpeed, ForceMode.Acceleration);
        }
    }
    void SetDirection(Vector2 vec)
    {
        _dir = new Vector2(vec.x, vec.y);
    }
}
