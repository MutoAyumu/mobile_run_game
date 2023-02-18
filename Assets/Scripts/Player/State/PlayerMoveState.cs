using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerMoveState : IState
{
    #region 変数
    InputType _inputType;
    #endregion

    #region プロパティ
    #endregion

    public PlayerMoveState()
    {
        InputSystemManager.Instance.JumpSub.Subscribe(_ => ChangeType(InputType.Jump));
        ActionSystem.Instance.Action.Subscribe(_ => ChangeType(InputType.Action));
    }

    public void OnEnter()
    {
        ChangeType(InputType.None);
    }

    public void OnExit()
    {
        
    }

    public int OnUpdate()
    {
        var id = (int)PlayerController.StateType.Move;

        if (_inputType == InputType.Jump)
        {
            id = (int)PlayerController.StateType.Jump;
        }
        if (_inputType == InputType.Action)
        {
            id = (int)PlayerController.StateType.Action;
        }

        return id;
    }

    void ChangeType(InputType type)
    {
        _inputType = type;
    }

    enum InputType
    {
        None = 0,
        Jump = 1,
        Action = 2,
    }
}
