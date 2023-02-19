using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAction : IAction
{
    [SerializeField] float _interval = 0.5f;
    [SerializeField] float _length = 0.2f;
    [SerializeField] int _lapNum = 6;
    [SerializeField] Vector2 _viewPort = new Vector2(0.5f, 0.5f);
    [SerializeField] AttackAction _prefab;

    float _rad;
    float _radDiff;
    int _failureCount;
    int _successCount;
    int _createCount;
    Vector2 _origin;
    Camera _cam;
    UnscaledTimer _timer = new UnscaledTimer();
    GenericObjectPool<AttackAction> _pool = new GenericObjectPool<AttackAction>();

    public int SuccessCount => _successCount;

    const int RAGIAN = 360;

    public CircleAction()
    {
        Debug.Log("CircleAction");
    }

    public void Init()
    {
        _cam = Camera.main;
        _origin = _cam.ViewportToScreenPoint(_viewPort);
        _radDiff = RAGIAN / _lapNum;

        var root = GameObject.FindGameObjectWithTag(IAction.ACTION_PARENT_TAG).transform;
        _pool.SetBaseObj(_prefab, root);
        _pool.SetCapacity(IAction.Limit);
    }

    public void Enter()
    {
        Init();
        _timer.Setup(_interval);
    }

    public void Exit()
    {
        
    }

    public bool Update()
    {
        if (_createCount < _lapNum)
        {
            if (_timer.RunTimer())
            {
                var obj = _pool.Instantiate();
                Create(obj);
                _createCount++;
            }
        }

        if (EndDecision()) return true;

        return false;
    }

    bool EndDecision()
    {
        var n = _successCount + _failureCount;

        if (n >= _lapNum) return true;

        return false;
    }
    
    void Create(AttackAction action)
    {
        var setPos = Vector2.zero;
        setPos.x = _origin.x + _length * Mathf.Cos(_rad);
        setPos.y = _origin.y + _length * Mathf.Sin(_rad);

        var dir = setPos - _origin;
        action.Setup(_origin, dir);
        action.Init(() => _successCount++, () => _failureCount++);

        _rad += _radDiff;
    }
}
