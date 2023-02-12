using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerJumpState : IState
{
    [Header("Jump")]
    [SerializeField] float _jumpPower = 1;

    Rigidbody _rb;
    Animator _anim;

    public int Type => (int)PlayerController.StateType.Jump;

    public void Init()
    {
        TryGetComponent(out _rb);
        TryGetComponent(out _anim);
    }

    public void OnEnter()
    {
        OnJump();
    }

    public int OnUpdate()
    {
        return (int)PlayerController.StateType.Move;
    }

    public void OnEixt()
    {

    }

    void OnJump()
    {
        var vel = _rb.velocity;
        vel.y = 0;
        _rb.velocity = vel;
        _rb.AddForce(Vector3.up * _jumpPower, ForceMode.VelocityChange);
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }
}