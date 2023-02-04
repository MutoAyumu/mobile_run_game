using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1;
    [SerializeField] float _jumpPower = 1;
    [Header("IsGroundCheck")]
    [SerializeField] float _groundCheckRadius = 1f;
    [SerializeField] LayerMask _groundLayer;

    float _currentSpeed;
    bool _isGroundChecked;
    Vector2 _dir;
    Rigidbody _rb;
    Animator _anim;
    Transform _thisTransform;

    const string JUMP_PARAM = "IsJump";
    const string GROUND_LAYER_NAME = "Ground";

    public void Init(PlayerController player)
    {
        TryGetComponent(out _rb);
        TryGetComponent(out _anim);
        TryGetComponent(out _thisTransform);

        player.Input.MoveVector.Subscribe(x => SetDirection(x)).AddTo(this);
        player.Input.JumpSub.Subscribe(_ => OnJump()).AddTo(this);
        player.OnUpdateSub.Subscribe(_ => OnUpdate()).AddTo(this);
        _currentSpeed = _moveSpeed;
    }
    private void Reset()
    {
        _groundLayer = LayerMask.GetMask(GROUND_LAYER_NAME);
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

        _isGroundChecked = IsGroundCheck();
        _anim.SetBool(JUMP_PARAM, !_isGroundChecked);
    }
    void OnJump()
    {
        if (!_isGroundChecked) return;

        var vel = _rb.velocity;
        vel.y = 0;
        _rb.velocity = vel;
        _rb.AddForce(Vector3.up * _jumpPower, ForceMode.VelocityChange);
    }
    bool IsGroundCheck()
    {
        var check = false;
        var hit = Physics.OverlapSphere(_thisTransform.position, _groundCheckRadius, _groundLayer);

        if(hit.Length > 0)
        {
            check = true;
        }

        return check;
    }
    void SetDirection(Vector2 vec)
    {
        _dir = new Vector2(vec.x, vec.y);
    }
    private void OnDrawGizmosSelected()
    {
        var pos = Application.isPlaying ? _thisTransform.position : this.transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pos, _groundCheckRadius);
    }
}
