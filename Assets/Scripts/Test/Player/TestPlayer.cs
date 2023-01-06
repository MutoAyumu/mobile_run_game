using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    InputPlayer _input;
    Vector2 _dir;
    Rigidbody _rb;
    [SerializeField]float _speed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _input = GetComponent<InputPlayer>();
    }

    private void FixedUpdate()
    {
        if(_input.MoveVector != Vector2.zero)
        {
            _dir = _input.MoveVector;
            _rb.AddForce(new Vector3(_dir.x,-_rb.velocity.y,_dir.y).normalized * _speed, ForceMode.Acceleration);
        }
    }
}
