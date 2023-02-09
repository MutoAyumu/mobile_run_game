using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyAttack))]
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    #region 変数
    readonly Subject<Unit> _updateSub = new Subject<Unit>();
    readonly Subject<Unit> _enableSub = new Subject<Unit>();
    readonly Subject<Unit> _disableSub = new Subject<Unit>();

    EnemyHealth _health;
    EnemyAttack _attack;
    LifeState _state;
    #endregion

    #region プロパティ
    public IObservable<Unit> OnUpdateSub => _updateSub.TakeUntilDestroy(this);
    public IObservable<Unit> OnEnableSub => _enableSub.TakeUntilDestroy(this);
    public IObservable<Unit> OnDisableSub => _disableSub.TakeUntilDestroy(this);

    public LifeState CurrentState { get => _state; set => _state = value; }
    #endregion

    private void Awake()
    {
        TryGetComponent(out _health);
        TryGetComponent(out _attack);

        Init();
    }
    void Init()
    {
        _health.Init(this);
        _attack.Init(this);
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
        _updateSub.OnNext(Unit.Default);
    }
}
public enum LifeState
{
    Normal,
    Dead
}
