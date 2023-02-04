using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

[RequireComponent(typeof(InputPlayer))]
[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    readonly Subject<Unit> _updateSub = new Subject<Unit>();
    readonly Subject<Unit> _enableSub = new Subject<Unit>();
    readonly Subject<Unit> _disableSub = new Subject<Unit>();

    InputPlayer _input;
    PlayerMove _move;
    PlayerAttack _attack;

    #region Property
    /// <summary>
    /// ÉvÉåÉCÉÑÅ[ä÷åWÇÃUpdeteèàóùÇÇ‹Ç∆ÇﬂÇΩObservable
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

        Init();
    }
    void Init()
    {
        _input.Init(this);
        _move.Init(this);
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
