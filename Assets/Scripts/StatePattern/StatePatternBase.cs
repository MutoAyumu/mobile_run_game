﻿using System.Collections.Generic;
using UnityEngine;

public class StatePatternBase<TOwner>
{
    public abstract class StateBase
    {
        public StatePatternBase<TOwner> StatePattern;
        protected TOwner Owner => StatePattern.Owner;

        public virtual void Init(TOwner owner) { }
        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }
    }

    TOwner Owner { get; }
    StateBase _currentState;
    StateBase _prevState;
    readonly Dictionary<int, StateBase> _states = new Dictionary<int, StateBase>();

    public StatePatternBase(TOwner owner)
    {
        Owner = owner;
    }

    public void Add<T>(int stateId) where T : StateBase, new()
    {
        if (_states.ContainsKey(stateId))
        {
            Debug.LogError("既に登録されたIDです : " + stateId);
            return;
        }

        var newState = new T
        {
            StatePattern = this
        };
        newState.Init(Owner);
        _states.Add(stateId, newState);
    }

    public void OnStart(int stateId)
    {
        if (!_states.TryGetValue(stateId, out var nextState))
        {
            Debug.LogError("指定IDがありません : " + stateId);
            return;
        }

        _currentState = nextState;
        _currentState.OnEnter();
    }

    public void OnUpdate()
    {
        _currentState.OnUpdate();
    }

    public void ChangeState(int stateId)
    {
        if (!_states.TryGetValue(stateId, out var nextState))
        {
            Debug.LogError("指定IDがありません : " + stateId);
            return;
        }

        Debug.Log($"CurrentState {_currentState} : NextState {nextState}");

        _prevState = _currentState;
        _currentState.OnExit();
        _currentState = nextState;
        _currentState.OnEnter();
    }

    public void ChangePrevState()
    {
        if (_prevState == null)
        {
            Debug.LogError("1つ前のステートがありません");
            return;
        }

        (_prevState, _currentState) = (_currentState, _prevState);
    }
}