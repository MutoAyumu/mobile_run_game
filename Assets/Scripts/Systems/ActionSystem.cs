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
    
    readonly static ActionSystem _instance = new ActionSystem();
    #endregion

    #region プロパティ
    public static ActionSystem Instance => _instance;
    public IObservable<Unit> IsCompleted => _isCompletedSub;
    public IObservable<Unit> Action => _actionSub;
    public int SuccessCount => _currentData.State.SuccessCount;
    #endregion

    public ActionSystem() 
    {
        Debug.Log("New ActionSystem");
    }

    public void Init(ActionSystemAttachment attachment)
    {
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                Update();
            }).AddTo(attachment);

        _isCompleted = true;
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
        _isCompleted = false;
        _actionSub.OnNext(Unit.Default);
        _currentData.State.Init();
    }

    void Update()
    {
        if (_isCompleted) return;

        Debug.Log("Update");

        var com = _currentData.State.Update();

        if (com == true)
        {
            _isCompleted = true;
            _isCompletedSub.OnNext(Unit.Default);
        }
    }
}
