using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : IState
{
    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }

    public int OnUpdate()
    {
        return (int)PlayerController.StateType.Move;
    }
}
