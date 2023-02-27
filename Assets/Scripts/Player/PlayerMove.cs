using UnityEngine;
using UniRx;

public class PlayerMove : MonoBehaviour
{
    #region 変数
    [Header("Parameter")]
    [SerializeField] float _horizontalMoveSpeed = 1;
    [SerializeField] float _verticalMoveSpeed = 3;

    Rigidbody _rb;
    Vector2 _dir;
    float _currentVerticalMoveSpeed;
    #endregion

    #region プロパティ

    #endregion

    #region 定数

    #endregion

    private void Awake()
    {
        TryGetComponent(out _rb);

        _currentVerticalMoveSpeed = _horizontalMoveSpeed;

        InputSystemManager.Instance.MoveVector.Subscribe(OnSetDirection).AddTo(this);
    }

    void OnSetDirection(Vector2 vec)
    {
        _dir = vec;
    }

    public void OnMove()
    {
        var vel = new Vector3(_dir.x, 0, 1).normalized;
        vel.x *= _horizontalMoveSpeed;
        vel.y = _rb.velocity.y;
        vel.z = _currentVerticalMoveSpeed;
        _rb.velocity = vel;
    }
}
