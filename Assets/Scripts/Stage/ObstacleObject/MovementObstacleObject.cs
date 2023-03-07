using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(HandlePoint))]
public class MovementObstacleObject : ObstacleObject
{
    [SerializeField] float _stopDistance = 0.5f;
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] float _rotateSpeed = 1f;

    Vector3[] _points;
    int _currentIndex;
    bool _isRotated;

    Rigidbody _rb;
    Transform _childTransform;
    Transform _parentTransform;

    private void Awake()
    {
        TryGetComponent(out _parentTransform);
        var child = _parentTransform.GetChild(0);
        child.TryGetComponent(out _rb);
        child.TryGetComponent(out _childTransform);

        var point = GetComponent<HandlePoint>().LocalNodes;
        var index = 0;
        _points = new Vector3[point.Length];

        foreach(var p in point)
        {
            _points[index] = _parentTransform.TransformPoint(p);
            index++;
        }

        Rotate();
    }
    private void Update()
    {
        if (_isRotated) return;

        var pos = _points[_currentIndex];
        var newPos = new Vector3(pos.x, _childTransform.position.y, pos.z);

        if(Vector3.Distance(_childTransform.position, newPos) < _stopDistance)
        {
            Rotate();
        }

        Move();
    }

    void Move()
    {
        var dir = _points[_currentIndex] - _childTransform.position;
        var move = new Vector3(dir.x, 0, dir.z).normalized;
        move *= _moveSpeed;
        move.y = _rb.velocity.y;

        _rb.velocity = move;
    }

    void Rotate()
    {
        _isRotated = true;
        var index = (_currentIndex + 1) % _points.Length;

        _childTransform.DOLookAt(_points[index], _rotateSpeed)
            .OnComplete(() =>
            {
                _currentIndex = index;
                _isRotated = false;
            });
    }
}