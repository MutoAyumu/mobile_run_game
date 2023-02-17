using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerAction))]
public class PlayerController : MonoBehaviour
{
    #region 変数
    StatePatternBase _statePattern;
    Transform _thisTransform;
    Animator _anim;
    PlayerMove _move;
    PlayerJump _jump;
    PlayerHealth _health;
    PlayerAction _action;
    #endregion

    #region プロパティ
    public StatePatternBase StatePattern => _statePattern;
    #endregion

    const string JUMP_PARAM = "IsJump";

    private void Awake()
    {
        TryGetComponent(out _move);
        TryGetComponent(out _jump);
        TryGetComponent(out _health);
        TryGetComponent(out _action);
        TryGetComponent(out _thisTransform);
        TryGetComponent(out _anim);

        Init();
    }

    void Init()
    {
        _statePattern = new StatePatternBase();
        _statePattern.Add<PlayerMoveState>((int)StateType.Move);
        _statePattern.Add<PlayerJumpState>((int)StateType.Jump);
        _statePattern.Add<PlayerActionState>((int)StateType.Action);
        _statePattern.Add<PlayerDeadState>((int)StateType.Dead);

        _jump.Init(this);
    }

    private void Start()
    {
        _statePattern.OnStart((int)StateType.Move);
    }

    private void Update()
    {
        _statePattern.OnUpdate();

        switch(_statePattern.CurrentStateID)
        {
            case (int)StateType.Move:
                _move.OnMove();
                break;
            case (int)StateType.Jump:
                _jump.OnJump();
                break;
            case (int)StateType.Action:
                break;
            case (int)StateType.Dead:
                break;
            default:
                break;
        }

        _anim.SetBool(JUMP_PARAM, !_jump.IsGround);
    }

    public enum StateType
    {
        Dead = -1,
        Move = 0,
        Action = 1,
        Jump = 2,
    }
}
