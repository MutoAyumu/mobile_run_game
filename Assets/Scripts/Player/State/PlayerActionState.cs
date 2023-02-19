using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerActionState : IState
{
    #region �ϐ�
    InputType _inputType;
    #endregion

    #region �v���p�e�B
    #endregion

    public PlayerActionState()
    {
        ActionSystem.Instance.IsCompleted.Subscribe(_ => _inputType = InputType.End);
    }

    public void OnEnter()
    {
        _inputType = InputType.Begin;
    }

    public void OnExit()
    {
        
    }

    public int OnUpdate()
    {
        var id = (int)PlayerController.StateType.Action;

        if(_inputType == InputType.End)
        {
            id = (int)PlayerController.StateType.Move;
        }

        return id;
    }

    enum InputType
    {
        Begin = 0,
        End = 1,
    }
}
