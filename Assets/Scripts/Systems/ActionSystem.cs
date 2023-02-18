using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ActionSystem
{
    #region 変数
    ActionData _currentData;
    bool _isCompleted;
    Subject<Unit> _actionSub = new Subject<Unit>();
    #endregion

    #region プロパティ
    public static ActionSystem Instance => new ActionSystem();
    public bool IsCompleted => _isCompleted;
    public IObservable<Unit> Action => _actionSub;
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

        _isCompleted = _currentData.ActionUpdate();
    }
}
