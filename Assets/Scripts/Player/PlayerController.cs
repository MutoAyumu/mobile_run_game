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
    [SerializeField] PlayerMoveState _moveState;
    [SerializeField] PlayerJumpState _jumpState;
    [SerializeField] PlayerActionState _actionState;
    [SerializeField] PlayerDeadState _deadState;

    [Header("Health")]
    [SerializeField] ReactiveProperty<float> _health = new ReactiveProperty<float>(3);

    readonly Subject<Unit> _enableSub = new Subject<Unit>();
    readonly Subject<Unit> _disableSub = new Subject<Unit>();

    StatePatternBase<PlayerController> _statePattern;
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
        TryGetComponent(out _anim);

        Init();
    }

    void Init()
    {
        _statePattern = new StatePatternBase<PlayerController>(this);
        _statePattern.Add((int)StateType.Move, _moveState);
        _statePattern.Add((int)StateType.Jump, _jumpState);
        _statePattern.Add((int)StateType.Action, _actionState);
        _statePattern.Add((int)StateType.Dead, _deadState);

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
            //_statePattern.ChangeState((int)StateType.Dead);
        }
    }

    private void OnDrawGizmosSelected()
    {
        var pos = Application.isPlaying ? _thisTransform.position : this.transform.position;

        Gizmos.color = _moveState.IsGroundChecked ? Color.green : Color.red;
        Gizmos.DrawWireSphere(pos, _moveState.Radius);
    }

    public enum StateType
    {
        Dead = -1,
        Move = 0,
        Action = 1,
        Jump = 2,
    }
}
