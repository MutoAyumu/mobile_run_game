using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public partial class EnemyController : MonoBehaviour, IDamage
{
    #region 変数
    [SerializeField] EnemyAttackState _attackState;
    [SerializeField] EnemyIdleState _IdleState;
    [SerializeField] EnemyDeadState _deadState;
    [SerializeField] FloatReactiveProperty _health = new FloatReactiveProperty(10);

    Animator _anim;
    StatePatternBase<EnemyController> _statePattern;
    #endregion

    #region プロパティ
    public IReadOnlyReactiveProperty<float> Health => _health;
    #endregion

    #region 定数
    const string TAKEDAMAGE_PARAM = "IsTakeDamage";
    const string DEATH_PARAM = "IsDead";
    #endregion

    private void Awake()
    {
        Init();
    }
    private void Start()
    {
        _statePattern.OnStart((int)StateType.Idle);
    }

    void Init()
    {
        TryGetComponent(out _anim);

        _statePattern = new StatePatternBase<EnemyController>(this);
        _statePattern.Add((int)StateType.Dead, _deadState);
        _statePattern.Add((int)StateType.Idle, _IdleState);
        _statePattern.Add((int)StateType.Attack, _attackState);
    }

    private void OnEnable()
    {
        FieldManager.Instance.Targets.Add(this);
    }

    private void OnDisable()
    {
        FieldManager.Instance.Targets.Remove(this);
    }

    private void Update()
    {
        _statePattern.OnUpdate();
    }

    public void TakeDamage(float damage)
    {
        if (_statePattern.CheckCurrentStateID((int)StateType.Dead) is null or true) return;

        _health.Value -= damage;

        if (_health.Value <= 0)
        {
            //_statePattern.ChangeState((int)StateType.Dead);
            _anim.SetTrigger(DEATH_PARAM);
            return;
        }

        _anim.SetTrigger(TAKEDAMAGE_PARAM);
    }
    public enum StateType
    {
        Dead = -1,
        Idle = 0,
        Attack = 1,
    }
}