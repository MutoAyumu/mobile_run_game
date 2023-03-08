using UnityEngine;
using UniRx;

public class PlayerMove : MonoBehaviour
{
    #region 変数
    [Header("Parameter")]
    [SerializeField] float _moveSpeed = 5;
    [SerializeField] float _rotateSpeed = 500;
    [SerializeField] float _rotateVelocityLimit = 0.5f;
    [SerializeField] float _dampSpeed = 0.1f;

    Rigidbody _rb;
    Vector2 _dir;
    Transform _transform;
    Animator _anim;
    float _currentMoveSpeed;
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

        _currentMoveSpeed = _moveSpeed;

        InputSystemManager.Instance.MoveVector.Subscribe(OnSetDirection).AddTo(this);
    }

    void OnSetDirection(Vector2 vec)
    {
        _dir = vec;
    }

    public void OnMove()
    {
        var vel = new Vector3(_dir.x, 0, _dir.y).normalized;
        var dir = Vector3.zero;

        OnRotate(vel);

        if (vel != Vector3.zero)
        {
            dir = _transform.forward * _currentMoveSpeed;
        }

        dir.y = _rb.velocity.y;
        _rb.velocity = dir;

        _anim.SetFloat(MOVE_ANIM_PARAM, vel.magnitude, _dampSpeed, Time.deltaTime);
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
}
