using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(TestInputPlayer))]
[RequireComponent(typeof(Rigidbody))]
public class TestPlayer : MonoBehaviour
{
    TestInputPlayer _input;
    Rigidbody _rb;
    TestPlayerMove _move;
    [SerializeField]float _speed;

    public float Speed => _speed;
    public Rigidbody Rb => _rb;

    private void Awake()
    {
        TryGetComponent(out _rb);
        TryGetComponent(out _input);
        TryGetComponent(out _move);

        Init();
        _move.Init(this);
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
