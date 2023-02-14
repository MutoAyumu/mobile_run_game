using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public class PlayerDeadState : IState
{
    #region 変数

    #endregion

    #region プロパティ
    public int Type => throw new System.NotImplementedException();
    #endregion

    public void Init()
    {
        
    }

    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public int OnUpdate()
    {
        throw new System.NotImplementedException();
    }
}