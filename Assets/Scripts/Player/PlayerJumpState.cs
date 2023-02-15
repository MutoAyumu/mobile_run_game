using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public class PlayerJumpState : IState
{
    #region ïœêî
    [Header("Parameter")]
    [SerializeField] float _jumpPower = 1;
    [Header("GameObject")]
    [SerializeField] GameObject _go;
    
    Rigidbody _rb;
    Animator _anim;
    #endregion

    public void Init()
    {
        _go.TryGetComponent(out _rb);
        _go.TryGetComponent(out _anim);
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
        
    }
}