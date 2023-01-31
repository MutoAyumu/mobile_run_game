using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

[RequireComponent(typeof(InputPlayer))]
[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    readonly Subject<Unit> _updateSub = new Subject<Unit>();
    readonly Subject<Unit> _enableSub = new Subject<Unit>();
    readonly Subject<Unit> _disableSub = new Subject<Unit>();

    InputPlayer _input;
    PlayerMove _move;
    Rigidbody _rb;

    #region Property
    /// <summary>
    /// ÉvÉåÉCÉÑÅ[ä÷åWÇÃUpdeteèàóùÇÇ‹Ç∆ÇﬂÇΩObservable
    /// </summary>
    public IObservable<Unit> OnUpdateSub => _updateSub;
    public IObservable<Unit> OnEnableSub => _enableSub;
    public IObservable<Unit> OnDisableSub => _disableSub;

    public Rigidbody Rigidbody => _rb;
    public InputPlayer Input => _input;
    #endregion

    private void Awake()
    {
        TryGetComponent(out _input);
        TryGetComponent(out _move);
        TryGetComponent(out _rb);

        Init();
    }
    void Init()
    {
        _input.Init(this);
        _move.Init(this);
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
