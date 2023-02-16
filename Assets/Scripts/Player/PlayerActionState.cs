using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UniRx;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UniRx.Triggers;

[System.Serializable]
public class PlayerActionState : IState
{
    #region　変数
    [Header("Parameter")]
    [SerializeField] ActionData _actionData;
    [SerializeField] AttackAction _action;
    [SerializeField] AttackType _attackType = AttackType.First;
    [SerializeField] bool _isDebug;

    int _tapCount;
    int _actionObjectCount;
    float _attackPower;

    GameObject _player;
    Animator _anim;
    Transform _actionParent;
    Transform[] _actionOrizinPositions;
    ReactiveProperty<bool> _isActed = new ReactiveProperty<bool>();
    #endregion

    #region　プロパティ
    public IReadOnlyReactiveProperty<bool> IsActed => _isActed;
    #endregion

    #region　定数
    const string ACTION_PARENT_TAG = "ActionParent";
    const string ACTION_OBJECT_POSITION = "ActionObjPos";
    const string ATTACK_ANIMATION_TAG = "Attack";
    const string ATTACK_INTEGER_PARAM = "AttackNumber";
    const string PLAYER_TAG = "Player";
    #endregion

    public void Init()
    {
        _actionParent = GameObject.FindGameObjectWithTag(ACTION_PARENT_TAG).transform;
        var objects = GameObject.FindGameObjectsWithTag(ACTION_OBJECT_POSITION);
        _actionOrizinPositions = Array.ConvertAll(objects, go => go.transform);
        _player = GameObject.FindGameObjectWithTag(PLAYER_TAG);

        InputSystemManager.Instance.TouchState.Subscribe(TapCount).AddTo(_player);
        _player.TryGetComponent(out _anim);
        SetAnimationTrigger();
    }

    public void OnEnter()
    {
        if (_isDebug)
            _tapCount = 100;
    }

    public int OnUpdate()
    {
        if(!_isActed.Value)
        {
            if(_tapCount >= 10)
            {
                OnAction();
                _tapCount -= 10;
                _isActed.Value = true;
                CameraManager.Instance.ChangePreferredOrder(VCameraType.Action);
                CameraManager.Instance.ChangeTimeScale(TimeScaleType.SlowTime);
            }
            else
            {
                return (int)PlayerController.StateType.Move;
            }
        }

        if (_actionObjectCount >= _actionOrizinPositions.Length)
        {
            OnAttack(_attackPower);
            return (int)PlayerController.StateType.Move;
        }

        return (int)PlayerController.StateType.Action;
    }

    public void OnExit()
    {
        _actionObjectCount = 0;
        _attackPower = 0;

        if (_isActed.Value)
        {
            _isActed.Value = false;
            CameraManager.Instance.ChangePreferredOrder(VCameraType.PlayerFollow);
            CameraManager.Instance.ChangeTimeScale(TimeScaleType.NormalTime);
        }
    }

    void TapCount(TouchState state)
    {
        if (state.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            _tapCount++;
        }
    }

    void SubscribeAction(AttackAction action)
    {
        action.Init(OnCount);
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
            }).AddTo(_player);
    }
    enum AttackType
    {
        None = 0,
        First = 1,
        Second = 2,
        Third = 3
    }
}