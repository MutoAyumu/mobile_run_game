using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ActionSystem
{
    #region 変数
    ActionData _currentData;
    bool _isCompleted = true;
    Subject<Unit> _actionSub = new Subject<Unit>();
    Subject<Unit> _isCompletedSub = new Subject<Unit>();
    #endregion

    #region プロパティ
    public static ActionSystem Instance => new ActionSystem();
    public IObservable<Unit> IsCompleted => _isCompletedSub;
    public IObservable<Unit> Action => _actionSub;
    public int SuccessCount => _currentData.State.SuccessCount;
    #endregion

    public ActionSystem() { }

    public void Init(PlayerAction player)
    {
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                Update();
            }).AddTo(player);
    }

    public void OnStart(ActionData data)
    {
        _currentData = data;
        _currentData.State.Enter();
    }

    public void ChangeActionData(ActionData data)
    {
        _currentData.State.Exit();
        _currentData = data;
        _currentData.State.Enter();
    }

    public void StartAction(PlayerAction player)
    {
        _actionSub.OnNext(Unit.Default);
        _isCompleted = false;
    }

    void Update()
    {
        if (_isCompleted) return;

        var com = _currentData.ActionUpdate();

        if(com != _isCompleted)
        {
            _isCompleted = true;
            _isCompletedSub.OnNext(Unit.Default);
        }
    }
}
