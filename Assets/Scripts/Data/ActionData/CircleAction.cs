using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAction : IAction
{
    [SerializeField] float _interval = 0.5f;
    [SerializeField] float _length = 0.2f;
    [SerializeField] Vector2 _viewPort = new Vector2(0.5f, 0.5f);
    [SerializeField] AttackAction _prefab;

    float _rad;
    Vector2 _origin;
    Camera _cam;
    UnscaledTimer _timer = new UnscaledTimer();
    GenericObjectPool<AttackAction> _pool = new GenericObjectPool<AttackAction>();

    public CircleAction()
    {
        Init();
    }

    public void Init()
    {
        _cam = Camera.main;
        _origin = _cam.ViewportToScreenPoint(_viewPort);

        var root = GameObject.FindGameObjectWithTag(IAction.ACTION_PARENT_TAG).transform;
        _pool.SetBaseObj(_prefab, root);
        _pool.SetCapacity(IAction.Limit);
    }

    public void Enter()
    {
        _timer.Setup(_interval);
    }

    public void Exit()
    {
        
    }

    public bool Update()
    {
        if(_timer.RunTimer())
        {
            var obj = _pool.Instantiate();
            Create(obj);
        }

        return false;
    }
    
    void Create(AttackAction action)
    {
        var setPos = Vector2.zero;
        setPos.x = _length * Mathf.Cos(_rad);
        setPos.y = _length * Mathf.Sin(_rad);

        var dir = setPos - _origin;
        action.Create(_origin, dir);

        _rad += 0.2f;
    }
}