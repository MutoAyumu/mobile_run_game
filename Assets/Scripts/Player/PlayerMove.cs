using UnityEngine;
using UniRx;

public class PlayerMove : MonoBehaviour
{
    #region 変数
    [Header("Parameter")]
    [SerializeField] float _moveSpeed = 1;

    Rigidbody _rb;
    Vector2 _dir;
    float _currentSpeed;
    #endregion

    #region プロパティ

    #endregion

    #region 定数

    #endregion

    private void Awake()
    {
        TryGetComponent(out _rb);

        _currentSpeed = _moveSpeed;

        InputSystemManager.Instance.MoveVector.Subscribe(OnSetDirection).AddTo(this);
    }
    
    void OnSetDirection(Vector2 vec)
    {
        _dir = vec;
    }

    public void OnMove()
    {
        if (_dir != Vector2.zero)
        {
            var vel = new Vector3(_dir.x, 0, 0).normalized * _currentSpeed;
            vel.y = _rb.velocity.y;
            _rb.velocity = vel;
        }
    }
}
