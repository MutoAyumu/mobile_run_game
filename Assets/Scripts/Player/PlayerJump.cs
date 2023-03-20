using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerJump : MonoBehaviour
{
    #region 変数
    [Header("Parameter")]
    [SerializeField] float _jumpPower = 1;
    [SerializeField] ParticleSystem _particlePrefab;

    [Header("IsGroundCheck")]
    [SerializeField] float _groundCheckRadius = 1f;

    bool _isGroundChecked;
    Rigidbody _rb;
    Transform _thisTransform;
    LayerMask _groundLayer;
    PlayerController _owner;
    ParticleSystem _particle;
    #endregion

    #region プロパティ
    public bool IsGround => IsGroundCheck();
    public ParticleSystem Particle => _particle;
    #endregion

    #region 定数
    const string GROUND_LAYER_NAME = "Ground";
    #endregion

    private void Awake()
    {
        TryGetComponent(out _rb);
        TryGetComponent(out _thisTransform); 
        _groundLayer = LayerMask.GetMask(GROUND_LAYER_NAME);
        _particle = Instantiate(_particlePrefab, _thisTransform.position, Quaternion.identity, _thisTransform);
        _particle.Play();
    }

    public void Init(PlayerController owner)
    {
        _owner = owner;
    }

    public void OnJump()
    {
        if (_owner.StatePattern.CurrentStateID != (int)PlayerController.StateType.Jump) return;

        if (!IsGround) return;

        _particle.Stop();

        var vel = _rb.velocity;
        vel.y = 0;
        _rb.velocity = vel;
        _rb.AddForce(Vector3.up * _jumpPower, ForceMode.VelocityChange);
    }

    bool IsGroundCheck()
    {
        var check = false;
        var hit = Physics.OverlapSphere(_thisTransform.position, _groundCheckRadius, _groundLayer);

        if (hit.Length > 0)
        {
            check = true;
        }

        return check;
    }

    private void OnDrawGizmosSelected()
    {
        var pos = Application.isPlaying ? _thisTransform.position : this.transform.position;

        Gizmos.color = _isGroundChecked ? Color.green : Color.red;
        Gizmos.DrawWireSphere(pos, _groundCheckRadius);
    }
}