using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAction : IAction
{
    [SerializeField] float _interval = 0.5f;
    [SerializeField] float _length = 0.2f;
    [SerializeField] Vector2 _viewPort = new Vector2(0.5f, 0.5f);
    [SerializeField] AttackAction _action;

    float _rad;
    Vector2 _origin;
    Camera _cam;
    UnscaledTimer _timer = new UnscaledTimer();

    public void Init()
    {
        _cam = Camera.main;
        _origin = _cam.ViewportToScreenPoint(_viewPort);
    }

    public void Enter()
    {
        _timer.Setup(_interval);
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        if(_timer.RunTimer())
        {
            //var action = GameObject.Instantiate()
            //Create(action);
        }
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
