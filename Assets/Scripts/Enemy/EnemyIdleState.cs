using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

[System.Serializable]
public class EnemyIdleState : IState
{
    #region �ϐ�
    [SerializeField] float _interval = 2f;

    Timer _intervalTimer = new Timer();
    GameObject _owner;
    #endregion

    #region �v���p�e�B
    #endregion

    #region �萔
    const string OWNER_TAG = "Enemy";
    #endregion

    public void Init()
    {
        _intervalTimer.Setup(_interval);
        _owner = GameObject.FindGameObjectWithTag(OWNER_TAG);
    }

    public void OnEnter()
    {
        
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
        
    }
}