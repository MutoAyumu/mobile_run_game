using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public class PlayerJumpState : IState
{
    #region �ϐ�
    [Header("Parameter")]
    [SerializeField] float _jumpPower = 1;

    GameObject _player;
    Rigidbody _rb;
    Animator _anim;
    #endregion

    #region �萔
    const string PLAYER_TAG = "Player";
    #endregion

    public void Init()
    {
        _player = GameObject.FindGameObjectWithTag(PLAYER_TAG);

        _player.TryGetComponent(out _rb);
        _player.TryGetComponent(out _anim);
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