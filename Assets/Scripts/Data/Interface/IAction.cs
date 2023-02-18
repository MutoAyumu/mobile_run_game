using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    const string ACTION_PARENT_TAG = "ActionParent";
    const int Limit = 15;

    public void Init();
    public void Enter();
    public bool Update();
    public void Exit();
}
