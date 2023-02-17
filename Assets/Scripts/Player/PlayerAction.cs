using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UniRx;
using System;
using System.Collections.Generic;
using UniRx.Triggers;

public class PlayerAction : MonoBehaviour
{
    #region　変数
    [Header("Parameter")]
    [SerializeField] ActionData _actionData;
    [SerializeField] AttackAction _action;
    [SerializeField] AttackType _attackType = AttackType.First;
    [SerializeField] bool _isDebug;

    int _tapCount = 10000;
    int _actionObjectCount;
    float _attackPower;

    Animator _anim;
    Transform _actionParent;
    Transform[] _actionOrizinPositions;
    PlayerController _owner;
    #endregion

    #region　プロパティ
    #endregion

    #region　定数
    const string ACTION_PARENT_TAG = "ActionParent";
    const string ACTION_OBJECT_POSITION = "ActionObjPos";
    const string ATTACK_ANIMATION_TAG = "Attack";
    const string ATTACK_INTEGER_PARAM = "AttackNumber";
    const string PLAYER_TAG = "Player";
    #endregion

    private void Awake()
    {
        _actionParent = GameObject.FindGameObjectWithTag(ACTION_PARENT_TAG).transform;
        var objects = GameObject.FindGameObjectsWithTag(ACTION_OBJECT_POSITION);
        _actionOrizinPositions = Array.ConvertAll(objects, go => go.transform);

        InputSystemManager.Instance.TouchState.Subscribe(TapCount).AddTo(this);
        InputSystemManager.Instance.ActionSub.Subscribe(_ => OnStart()).AddTo(this);
        
        SetAnimationTrigger();
    }

    public void Init(PlayerController owner)
    {
        _owner = owner;
    }

    void OnStart()
    {
        if (_tapCount >= 10)
        {
            OnAction();
            _tapCount -= 10;
            CameraManager.Instance.ChangePreferredOrder(VCameraType.Action);
            CameraManager.Instance.ChangeTimeScale(TimeScaleType.SlowTime);
        }
    }

    public void OnFinish()
    {
        _actionObjectCount = 0;
        _attackPower = 0;
        CameraManager.Instance.ChangePreferredOrder(VCameraType.PlayerFollow);
        CameraManager.Instance.ChangeTimeScale(TimeScaleType.NormalTime);
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
            var obj = Instantiate(_action, list[r].position, Quaternion.identity, _actionParent);
            obj.name = $"Action[{r}]";
            SubscribeAction(obj);
            list.RemoveAt(r);
        }
    }

    void OnCount(float power)
    {
        _actionObjectCount++;
        _attackPower += power;

        CheckCount();
    }
    
    void CheckCount()
    {
        if (_actionObjectCount >= _actionOrizinPositions.Length)
        {
            OnAttack(_attackPower);
            OnFinish();
        }
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
        TryGetComponent(out _anim);
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
            }).AddTo(this);
    }
    enum AttackType
    {
        None = 0,
        First = 1,
        Second = 2,
        Third = 3
    }
}