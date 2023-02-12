using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour, IDamage
{
    #region 変数
    PlayerMoveState _moveState;
    PlayerJumpState _jumpState;
    PlayerActionState _actionState;

    [Header("Health")]
    [SerializeField] ReactiveProperty<float> _health = new ReactiveProperty<float>(3);

    readonly Subject<Unit> _enableSub = new Subject<Unit>();
    readonly Subject<Unit> _disableSub = new Subject<Unit>();

    InputSystemManager _input;
    StatePatternBase<PlayerController> _statePattern;
    Rigidbody _rb;
    Animator _anim;
    Transform _thisTransform;
    #endregion

    #region プロパティ
    public IObservable<Unit> OnEnableSub => _enableSub.TakeUntilDestroy(this);
    public IObservable<Unit> OnDisableSub => _disableSub.TakeUntilDestroy(this);
    public IReadOnlyReactiveProperty<float> Health => _health;
    #endregion

    const string TAKEDAMAGE_PARAM = "IsTakeDamage";

    private void Awake()
    {
        TryGetComponent(out _rb);
        TryGetComponent(out _anim);

        Init();
    }

    void Init()
    {
        _statePattern = new StatePatternBase<PlayerController>(this);
        _statePattern.Add<PlayerMoveState>((int)StateType.Move);
        _statePattern.Add<PlayerJumpState>((int)StateType.Jump);
        _statePattern.Add<PlayerActionState>((int)StateType.Action);
        _statePattern.Add<PlayerDeadState>((int)StateType.Dead);

        _thisTransform = this.transform;
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

    public void TakeDamage(float damage)
    {
        _health.Value -= damage;
        _anim.SetTrigger(TAKEDAMAGE_PARAM);

        if(_health.Value <= 0)
        {
            _statePattern.ChangeState((int)StateType.Dead);
        }
    }

    private void OnDrawGizmosSelected()
    {
        var pos = Application.isPlaying ? _thisTransform.position : this.transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pos, _jumpState. _groundCheckRadius);
    }

    public enum StateType
    {
        Dead = -1,
        Move = 0,
        Action = 1,
        Jump = 2,
    }
}
