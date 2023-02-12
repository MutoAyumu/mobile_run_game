using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public int Type { get; }
    public void Init();
    public void OnEnter();
    public int OnUpdate();
    public void OnExit();
}
