using UnityEngine;
using UniRx;

public class PlayerMove : MonoBehaviour
{
    #region 変数
    [Header("Parameter")]
    [SerializeField] float _groundMoveSpeed = 5;
    [SerializeField] float _airMoveSpeed = 2;
    [SerializeField] float _forceMultiplier = 5f;
    [SerializeField] float _rotateSpeed = 500;
    [SerializeField] float _rotateVelocityLimit = 0.5f;
    [SerializeField] float _dampSpeed = 0.1f;

    Rigidbody _rb;
    Vector2 _dir;
    PlayerJump _jump;
    Transform _transform;
    Animator _anim;
    Timer _inputInvalidationTimer = new Timer();
    float _currentMoveSpeed;
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
    }

    void OnSetDirection(Vector2 vec)
    {
        _dir = vec;
    }

    public void OnMove()
    {
        if (!_isAccelerator)
        {
            var vel = new Vector3(_dir.x, 0, _dir.y).normalized;
            var dir = Vector3.zero;

            OnRotate(vel);

            if (vel != Vector3.zero)
            {
                _currentMoveSpeed = _jump.IsGround ? _groundMoveSpeed : _airMoveSpeed;
                dir = _transform.forward * _currentMoveSpeed;
            }

            var move = _forceMultiplier * (dir - _rb.velocity);
            move.y = _rb.velocity.y;

            _rb.AddForce(move, ForceMode.Acceleration);
        }
        else
        {
            if(_inputInvalidationTimer.RunTimer())
            {
                _isAccelerator = false;
            }
        }
        
        _anim.SetFloat(MOVE_ANIM_PARAM, _rb.velocity.magnitude, _dampSpeed, Time.deltaTime);
    }

    void OnRotate(Vector3 vel)
    {
        var rot = _transform.rotation;
        var speed = _rotateSpeed * Time.deltaTime;

        if (vel.magnitude > _rotateVelocityLimit)
        {
            rot = Quaternion.LookRotation(vel, Vector3.up);
        }

        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, rot, speed);
    }
    
    public void Accelerator(Vector3 vec, Vector3 rot, float inputInvalidationTime)
    {
        _isAccelerator = true;
        _inputInvalidationTimer.Setup(inputInvalidationTime);
  
        _rb.velocity = Vector3.zero;
        _rb.AddForce(vec, ForceMode.VelocityChange);

        _transform.rotation = Quaternion.LookRotation(rot, Vector3.up);
    }
}
