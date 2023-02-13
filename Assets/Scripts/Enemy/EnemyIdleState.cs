using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EnemyIdleState : IState
{
    #region 変数
    [Header("Idle")]
    [SerializeField] float _interval = 2f;

    Timer _intervalTimer = new Timer();
    #endregion

    #region プロパティ
    #endregion

    public void Init()
    {
        _intervalTimer.Setup(_interval);
    }

    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public int OnUpdate()
    {
        if (_intervalTimer.RunTimer())
        {
            return (int)EnemyController.StateType.Attack;
        }

        return (int)EnemyController.StateType.Idle;
    }
    public void OnExit()
    {
        throw new System.NotImplementedException();
    }
}