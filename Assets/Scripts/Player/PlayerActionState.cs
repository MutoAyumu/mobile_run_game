using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UniRx;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UniRx.Triggers;

public class PlayerActionState : IState
{
    [SerializeField] AttackAction _action;
    [SerializeField] AttackType _attackType = AttackType.First;
    [SerializeField] Image _touchEffect;

    Camera _cam;
    int _tapCount;
    int _actionObjectCount;
    float _attackPower;
    Transform _actionParent;
    Transform[] _actionOrizinPositions;
    Animator _anim;
    ReactiveProperty<bool> _isActed = new ReactiveProperty<bool>();

    public IReadOnlyReactiveProperty<bool> IsActed => _isActed;

    public int Type => (int)PlayerController.StateType.Action;

    const string ACTION_PARENT_TAG = "ActionParent";
    const string ACTION_OBJECT_POSITION = "ActionObjPos";
    const string ATTACK_ANIMATION_TAG = "Attack";
    const string ATTACK_INTEGER_PARAM = "AttackNumber";

    public void Init()
    {
        //_touchEffect.enabled = false;

        Owner.TryGetComponent(out _anim);
        _cam = Camera.main;
        SetAnimationTrigger();
        _actionParent = GameObject.FindGameObjectWithTag(ACTION_PARENT_TAG).transform;
        var objects = GameObject.FindGameObjectsWithTag(ACTION_OBJECT_POSITION);
        _actionOrizinPositions = Array.ConvertAll(objects, go => go.transform);
    }

    public void OnEnter()
    {
        OnAction();
        _tapCount -= 10;
        _isActed.Value = true;
        CameraManager.Instance.ChangePreferredOrder(VCameraType.Action);
        CameraManager.Instance.ChangeTimeScale(TimeScaleType.SlowTime);
    }

    public int OnUpdate()
    {
        if (_actionObjectCount >= _actionOrizinPositions.Length)
        {
            OnAttack(_attackPower);
            return (int)PlayerController.StateType.Move;
        }

        return (int)PlayerController.StateType.Action;
    }

    public void OnExit()
    {
        _isActed.Value = false;
        _actionObjectCount = 0;
        _attackPower = 0;
        CameraManager.Instance.ChangePreferredOrder(VCameraType.PlayerFollow);
        CameraManager.Instance.ChangeTimeScale(TimeScaleType.NormalTime);
    }

    void IsInProgress()
    {
        //ˆÚA—\’è
        var state = InputSystemManager.Instance.TouchState;

        if (state.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            //_touchEffect.enabled = true;
            _tapCount++;
        }
        else if (state.phase == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            //_touchEffect.enabled = false;
        }

        if (state.isInProgress)
        {
            _touchEffect.transform.position = state.position;
        }
    }

    void SubscribeAction(AttackAction action)
    {
        action.Init(x => OnCount(x));
    }

    void OnAction()
    {
        var list = new List<Transform>(_actionOrizinPositions);

        for (int i = 0; i < _actionOrizinPositions.Length; i++)
        {
            var r = UnityEngine.Random.Range(0, list.Count);
            var obj = GameObject.Instantiate(_action, list[r].position, Quaternion.identity, _actionParent);
            obj.name = $"Action[{r}]";
            SubscribeAction(obj);
            list.RemoveAt(r);
        }
    }

    void OnCount(float power)
    {
        _actionObjectCount++;
        _attackPower += power;
    }
    void OnAttack(float power)
    {
        var targets = FieldManager.Instance.Targets;

        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].TakeDamage(power);
        }

        _anim.SetInteger(ATTACK_INTEGER_PARAM, (int)_attackType);
    }
    void SetAnimationTrigger()
    {
        var trigger = _anim.GetBehaviour<ObservableStateMachineTrigger>();

        trigger
            .OnStateExitAsObservable()
            .Subscribe(stateInfo =>
            {
                var info = stateInfo.StateInfo;

                if (info.IsTag(ATTACK_ANIMATION_TAG))
                {
                    _anim.SetInteger(ATTACK_INTEGER_PARAM, (int)AttackType.None);
                }
            }).AddTo(Owner);
    }
    enum AttackType
    {
        None = 0,
        First = 1,
        Second = 2,
        Third = 3
    }
}