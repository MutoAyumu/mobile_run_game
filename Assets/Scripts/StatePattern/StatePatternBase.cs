using System.Collections.Generic;
using UnityEngine;

public class StatePatternBase
{
    int _currentStateID;
    IState _currentState;
    IState _prevState;
    readonly Dictionary<int, IState> _states = new Dictionary<int, IState>();

    public int CurrentStateID => _currentStateID;

    public void Add<T>(int stateId) where T : IState, new()
    {
        if (_states.ContainsKey(stateId))
        {
            Debug.LogError("既に登録されたIDです : " + stateId);
            return;
        }

        var newState = new T();

        _states.Add(stateId, newState);
    }

    public void OnStart(int stateId)
    {
        if (!_states.TryGetValue(stateId, out var nextState))
        {
            Debug.LogError("指定IDがありません : " + stateId);
            return;
        }

        _currentStateID = stateId;
        _currentState = nextState;
        _currentState.OnEnter();
    }

    public void OnUpdate()
    {
        var next = _currentState.OnUpdate();

        if(_currentState != _states[next])
        {
            ChangeState(next);
        }
    }

    public bool? CheckCurrentStateID(int stateId)
    {
        if(!_states.TryGetValue(stateId, out var state))
        {
            Debug.LogError("指定IDがありません : " + stateId);
            return null;
        }
        else if(state != _currentState)
        {
            Debug.LogError("指定IDと現在のStateIDが違います : " + stateId);
            return false;
        }

        return true;
    }

    private void ChangeState(int stateId)
    {
        if (!_states.TryGetValue(stateId, out var nextState))
        {
            Debug.LogError("指定IDがありません : " + stateId);
            return;
        }

        Debug.Log($"CurrentState {_currentState} : NextState {nextState}");

        _currentStateID = stateId;
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