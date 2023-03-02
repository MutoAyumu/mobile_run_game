using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAction : IAction
{
    [Header("Settings")]
    [SerializeField] int _requiredTapCount = 5;
    [SerializeField] float _generateInterval = 0.5f;
    [SerializeField] float _length = 200f;
    [SerializeField] int _countGeneratedInOnce = 9;
    [SerializeField] Vector2[] _viewPortPositions = new Vector2[1] { new Vector2(0.5f, 0.5f) };
    [Header("ActionPrefab")]
    [SerializeField] AttackAction _prefab;
    [SerializeField] float _destroyTime = 2.5f;
    [SerializeField] float _moveTime = 1f;
    [SerializeField] SkillObject _swordPrefab;

    float _radDiff;
    float _rad;
    int _repeatedCount;
    int _failureCount;
    int _successCount;
    int _createCount;
    Vector2[] _origins;
    Camera _cam;
    UnscaledTimer _timer = new UnscaledTimer();
    GenericObjectPool<AttackAction> _pool = new GenericObjectPool<AttackAction>();

    public int SuccessCount => _successCount;
    public int RequiredTapCount => _requiredTapCount;
    public SkillObject Skill => _swordPrefab;

    const int RAGIAN = 360;

    public CircleAction()
    {
        Debug.Log(typeof(Spike));
    }

    public void Init()
    {
        _successCount = 0;
        _failureCount = 0;
        _createCount = 0;
        _rad = 0;
        _repeatedCount = 0;
    }

    public void Enter()
    {
        _timer.Setup(_generateInterval);
        Setup();
    }

    public void Exit()
    {

    }

    public bool Update()
    {
        if (_repeatedCount < _viewPortPositions.Length)
        {

            if (_timer.RunTimer())
            {
                var obj = _pool.Instantiate();
                Create(obj);
                _createCount++;

                if (_createCount >= _countGeneratedInOnce)
                {
                    _createCount = 0;
                    _repeatedCount++;
                }
            }
        }

        if (EndDecision()) return true;

        return false;
    }

    void Setup()
    {
        _cam = Camera.main;
        _origins = new Vector2[_viewPortPositions.Length];

        for (int i = 0; i < _viewPortPositions.Length; i++)
        {
            _origins[i] = _cam.ViewportToScreenPoint(_viewPortPositions[i]);
        }

        _radDiff = RAGIAN / _countGeneratedInOnce;

        var root = GameObject.FindGameObjectWithTag(IAction.ACTION_PARENT_TAG).transform;
        _pool.SetBaseObj(_prefab, root);
        _pool.SetCapacity(IAction.Limit);
    }

    bool EndDecision()
    {
        var n = _successCount + _failureCount;

        if (n >= _countGeneratedInOnce * _viewPortPositions.Length) return true;

        return false;
    }

    void Create(AttackAction action)
    {
        var rad = _rad * Mathf.Deg2Rad;
        var dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        action.Setup(_origins[_repeatedCount], dir * _length, _destroyTime, _moveTime);
        action.Init(() => _successCount++, () => _failureCount++);
        _rad += _radDiff;
    }
}
