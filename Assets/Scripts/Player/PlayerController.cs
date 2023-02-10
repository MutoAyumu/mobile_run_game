using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

[RequireComponent(typeof(InputPlayer))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public partial class PlayerController : MonoBehaviour
{
    #region 変数
    readonly Subject<Unit> _enableSub = new Subject<Unit>();
    readonly Subject<Unit> _disableSub = new Subject<Unit>();

    InputPlayer _input;
    PlayerAttack _attack;
    PlayerHealth _health;
    StatePatternBase<PlayerController> _statePattern;
    Rigidbody _rb;
    Animator _anim;
    Transform _thisTransform;
    #endregion

    #region プロパティ
    public IObservable<Unit> OnEnableSub => _enableSub.TakeUntilDestroy(this);
    public IObservable<Unit> OnDisableSub => _disableSub.TakeUntilDestroy(this);
    public InputPlayer Input => _input;
    #endregion

    private void Awake()
    {
        TryGetComponent(out _input);
        TryGetComponent(out _attack);
        TryGetComponent(out _health);
        TryGetComponent(out _rb);
        TryGetComponent(out _anim);

        Init();
    }
    void Init()
    {
        _statePattern = new StatePatternBase<PlayerController>(this);
        _statePattern.Add<PlayerMove>((int)StateType.Move);
        _statePattern.Add<PlayerJump>((int)StateType.Jump);

        _thisTransform = this.transform;

        _input.Init(this);
        _attack.Init(this);
        _health.Init(this);
    }
    private void Start()
    {
        _statePattern.OnStart((int)StateType.Move);
    }
    private void OnEnable()
    {
        _enableSub.OnNext(Unit.Default);
    }
    private void OnDisable()
    {
        _disableSub.OnNext(Unit.Default);
    }
    private void Update()
    {
        _statePattern.OnUpdate();
    }
    enum StateType
    {
        Dead = -1,
        Move = 0,
        Run = 1,
        Attack = 2,
        Jump = 3,
    }
}
