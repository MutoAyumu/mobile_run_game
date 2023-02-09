using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

[RequireComponent(typeof(InputPlayer))]
[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    #region 変数
    readonly Subject<Unit> _updateSub = new Subject<Unit>();
    readonly Subject<Unit> _enableSub = new Subject<Unit>();
    readonly Subject<Unit> _disableSub = new Subject<Unit>();

    InputPlayer _input;
    PlayerMove _move;
    PlayerAttack _attack;
    PlayerHealth _health;
    #endregion

    #region プロパティ
    /// <summary>
    /// プレイヤー関係のUpdete処理をまとめたObservable
    /// </summary>
    public IObservable<Unit> OnUpdateSub => _updateSub.TakeUntilDestroy(this);
    public IObservable<Unit> OnEnableSub => _enableSub.TakeUntilDestroy(this);
    public IObservable<Unit> OnDisableSub => _disableSub.TakeUntilDestroy(this);
    public InputPlayer Input => _input;
    #endregion

    private void Awake()
    {
        TryGetComponent(out _input);
        TryGetComponent(out _move);
        TryGetComponent(out _attack);
        TryGetComponent(out _health);

        Init();
    }
    void Init()
    {
        _input.Init(this);
        _move.Init(this);
        _attack.Init(this);
        _health.Init(this);
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
