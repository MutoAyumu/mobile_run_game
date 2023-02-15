using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    public void Init();
    public void Enter();
    public void Update(AttackAction action);
    public void Exit();
}
