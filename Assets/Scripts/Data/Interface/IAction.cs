using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    public void Init();
    public void Enter();
    public bool Update();
    public void Exit();
}
