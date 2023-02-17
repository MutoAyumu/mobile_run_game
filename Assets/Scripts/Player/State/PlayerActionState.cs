using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionState : IState
{
    #region �ϐ�
    InputType _inputType;
    #endregion

    #region �v���p�e�B
    #endregion

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

        if(_inputType == InputType.Begin)
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
