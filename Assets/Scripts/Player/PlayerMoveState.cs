using UnityEngine;
using UniRx;

[System.Serializable]
public class PlayerMoveState : IState
{
    #region 変数
    [Header("Parameter")]
    [SerializeField] float _moveSpeed = 1;
    [Header("IsGroundCheck")]
    [SerializeField] float _groundCheckRadius = 1f;
    [Header("GameObject")]
    [SerializeField] GameObject _go;

    Rigidbody _rb;
    Animator _anim;
    Transform _thisTransform;
    LayerMask _groundLayer;
    InputType _inputType;
    Vector2 _dir;
    float _currentSpeed;
    bool _isGroundChecked;
    #endregion

    #region プロパティ
    public bool IsGroundChecked => _isGroundChecked;
    public float Radius => _groundCheckRadius;
    #endregion

    #region 定数
    const string JUMP_PARAM = "IsJump";
    const string GROUND_LAYER_NAME = "Ground";
    #endregion

    public void Init()
    {
        _go.TryGetComponent(out _rb);
        _go.TryGetComponent(out _anim);
        _go.TryGetComponent(out _thisTransform);
        _currentSpeed = _moveSpeed;
        _groundLayer = LayerMask.GetMask(GROUND_LAYER_NAME);
        InputSystemManager.Instance.JumpSub.Subscribe(_ => OnJump()).AddTo(_go);
        InputSystemManager.Instance.ActionSub.Subscribe(_ => OnAction()).AddTo(_go);
        InputSystemManager.Instance.MoveVector.Subscribe(OnSetDirection).AddTo(_go);
    }
    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        _inputType = InputType.None;
    }
    public int OnUpdate()
    {
        var id = OnMove();

        if(_inputType == InputType.Jump)
        {
            id = (int)PlayerController.StateType.Jump;
        }
        else if(_inputType == InputType.Action)
        {
            id = (int)PlayerController.StateType.Action;
        }

        return id;
    }
    
    void OnSetDirection(Vector2 vec)
    {
        _dir = vec;
    }

    int OnMove()
    {
        if (_dir != Vector2.zero)
        {
            var vel = new Vector3(_dir.x, 0, 0).normalized * _currentSpeed;
            vel.y = _rb.velocity.y;
            _rb.velocity = vel;
        }

        _isGroundChecked = IsGroundCheck();
        _anim.SetBool(JUMP_PARAM, !_isGroundChecked);

        return (int)PlayerController.StateType.Move;
    }

    void OnAction()
    {
        _inputType = InputType.Action;
    }

    void OnJump()
    {
        if (!_isGroundChecked) return;// || Owner._statePattern.CheckCurrentStateID((int)PlayerController.StateType.Action) is null or true) return;

        _inputType = InputType.Jump;
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

    enum InputType
    {
        None = 0,
        Jump = 1,
        Action = 2,
    }
}
