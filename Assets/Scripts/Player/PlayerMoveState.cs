using UnityEngine;
using UniRx;

[System.Serializable]
public class PlayerMoveState : IState
{
    [Header("Move")]
    [SerializeField] float _moveSpeed = 1;
    [Header("IsGroundCheck")]
    [SerializeField] float _groundCheckRadius = 1f;

    bool _isGroundChecked;
    LayerMask _groundLayer;
    float _currentSpeed;
    Rigidbody _rb;
    Animator _anim;
    InputType _inputType;
    Transform _thisTransform;

    const string JUMP_PARAM = "IsJump";
    const string GROUND_LAYER_NAME = "Ground";

    public void Init()
    {
        _currentSpeed = _moveSpeed;
        TryGetComponent(out _rb);
        TryGetComponent(out _anim);
        TryGetComponent(out _thisTransform);
        _groundLayer = LayerMask.GetMask(GROUND_LAYER_NAME);
        InputSystemManager.Instance.JumpSub.Subscribe(_ => OnJump()).AddTo(Owner);
        InputSystemManager.Instance.ActionSub.Subscribe(_ => OnAction()).AddTo(Owner);
    }
    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
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
    int OnMove()
    {
        var dir = InputSystemManager.Instance.MoveVector;

        if (dir != Vector2.zero)
        {
            var vel = new Vector3(dir.x, 0, 0).normalized * _currentSpeed;
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
        if (!_isGroundChecked)// || Owner._statePattern.CheckCurrentStateID((int)PlayerController.StateType.Action) is null or true) return;

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
