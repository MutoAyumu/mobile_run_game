using UnityEngine;
using UniRx;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    #region 変数
    [Header("Parameter")]
    [SerializeField] float _groundMoveSpeed = 5;
    [SerializeField] float _airMoveSpeed = 2;
    [SerializeField] float _forceMultiplier = 5f;
    [SerializeField] float _rotateSpeed = 500;
    [SerializeField] float _rotateVelocityLimit = 0.5f;
    [SerializeField] float _acceleratorDuration = 1f;
    [SerializeField] float _dampSpeed = 0.1f;

    Rigidbody _rb;
    Vector2 _dir;
    PlayerJump _jump;
    Transform _transform;
    Animator _anim;
    Timer _inputInvalidationTimer = new Timer();
    float _currentMoveSpeed;
    float _currentRotateSpeed;
    bool _isAccelerator;

    #endregion

    #region プロパティ

    #endregion

    #region 定数
    const string MOVE_ANIM_PARAM = "MoveSpeed";
    #endregion

    private void Awake()
    {
        TryGetComponent(out _rb);
        TryGetComponent(out _transform);
        TryGetComponent(out _anim);
        TryGetComponent(out _jump);

        InputSystemManager.Instance.MoveVector.Subscribe(OnSetDirection).AddTo(this);

        _currentRotateSpeed = _rotateSpeed;
    }

    void OnSetDirection(Vector2 vec)
    {
        _dir = vec;
    }

    public void OnMove()
    {
        var vel = new Vector3(_dir.x, 0, _dir.y).normalized;
        var dir = Vector3.zero;

        if (!_isAccelerator)
        {
            if (vel != Vector3.zero)
            {
                _currentMoveSpeed = _jump.IsGround ? _groundMoveSpeed : _airMoveSpeed;
                dir = _transform.forward * _currentMoveSpeed;
            }

            var move = _forceMultiplier * (dir - new Vector3(_rb.velocity.x, 0, _rb.velocity.z));

            _rb.AddForce(move, ForceMode.Acceleration);
            OnRotate(vel);
        }
        else
        {
            if(_inputInvalidationTimer.RunTimer())
            {
                _isAccelerator = false;
                DOVirtual.Float(0, _rotateSpeed, _acceleratorDuration, value => _currentRotateSpeed = value);
            }

            if(vel != Vector3.zero)
            {
                _currentMoveSpeed = _jump.IsGround ? _groundMoveSpeed : _airMoveSpeed;
                dir = new Vector3(vel.x, 0, 0) * _currentMoveSpeed;
            }

            _rb.AddForce(dir, ForceMode.Acceleration);
        }
        
        _anim.SetFloat(MOVE_ANIM_PARAM, _rb.velocity.magnitude, _dampSpeed, Time.deltaTime);
    }

    void OnRotate(Vector3 vel)
    {
        var rot = _transform.rotation;
        var speed = _currentRotateSpeed * Time.deltaTime;

        if (vel.magnitude > _rotateVelocityLimit)
        {
            rot = Quaternion.LookRotation(vel, Vector3.up);
        }

        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, rot, speed);
    }
    
    public void Accelerator(Vector3 vec, Quaternion rot, float inputInvalidationTime)
    {
        _isAccelerator = true;
        _inputInvalidationTimer.Setup(inputInvalidationTime);
  
        _rb.velocity = Vector3.zero;
        _rb.AddForce(vec, ForceMode.VelocityChange);

        _transform.rotation = rot;
    }
}
