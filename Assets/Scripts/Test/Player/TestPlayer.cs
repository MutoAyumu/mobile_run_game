using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(InputPlayer))]
[RequireComponent(typeof(Rigidbody))]
public class TestPlayer : MonoBehaviour
{
    InputPlayer _input;
    Rigidbody _rb;

    TestPlayerMove _move;

    [SerializeField]float _speed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _input = GetComponent<InputPlayer>();
        _move = new TestPlayerMove(_speed, _rb);

        Init();
    }

    private void FixedUpdate()
    {
        _move.OnMove();
    }

    void Init()
    {
        _input.MoveVector.Subscribe(x =>
        {
            _move.SetDirection(x);
        }).AddTo(this);
    }
}
