using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBase<TOwner> : IState
{

    public StatePatternBase<TOwner> StatePattern;

    protected TOwner Owner => StatePattern.Owner;

    public int Type => 0;

    public virtual void Init() { }

    public virtual void OnEnter() { }

    public virtual void OnExit() { }

    public virtual int OnUpdate() { return 0; }
}
