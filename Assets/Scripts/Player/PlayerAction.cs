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
    [SerializeField] AttackType _attackType = AttackType.First;
    [SerializeField] float _power = 1f;
    [SerializeField] bool _isDebug;

    float _tapCount = 10000;
    bool _isCompleted = true;

    Animator _anim;
    PlayerController _owner;
    #endregion

    #region　プロパティ
    #endregion

    #region　定数
    const string ACTION_PARENT_TAG = "ActionParent";
    const string ACTION_OBJECT_POSITION = "ActionObjPos";
    const string ATTACK_ANIMATION_TAG = "Attack";
    const string ATTACK_INTEGER_PARAM = "AttackNumber";
    #endregion

    private void Awake()
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isRemoteConnected)
        {
            InputSystemManager.Instance.TouchState.Subscribe(TapCount).AddTo(this);
        }
        else
        {
            InputSystemManager.Instance.EditorTouchButton.Subscribe(_ => AddTapCount(0.5f)).AddTo(this);
        }
#endif
#if UNITY_ANDROID
        InputSystemManager.Instance.TouchState.Subscribe(TapCount).AddTo(this);
#endif

        InputSystemManager.Instance.ActionSub.Subscribe(_ => OnStart()).AddTo(this);
        ActionSystem.Instance.IsCompleted.Subscribe(_ =>
        { 
            OnAttack();
            _isCompleted = true;
        });
        ActionSystem.Instance.OnStart(_actionData);

        if(!_isDebug)
        {
            _tapCount = 0;
        }

        SetAnimationTrigger();
    }

    public void Init(PlayerController owner)
    {
        _owner = owner;
    }

    void OnStart()
    {
        if (!_isCompleted) return;

        if (_tapCount >= 10)
        {
            _isCompleted = false;
            _tapCount -= 10;
            ActionSystem.Instance.StartAction(this);
            CameraManager.Instance.ChangePreferredOrder(VCameraType.Action);
            CameraManager.Instance.ChangeTimeScale(TimeScaleType.SlowTime);
        }
    }

    void OnFinish()
    {
        CameraManager.Instance.ChangePreferredOrder(VCameraType.PlayerFollow);
        CameraManager.Instance.ChangeTimeScale(TimeScaleType.NormalTime);
    }

    void TapCount(TouchState state)
    {
        if (state.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            AddTapCount(1);
        }
    }
    void AddTapCount(float add)
    {
        _tapCount += add;
    }    
    void OnAttack()
    {
        var power = _power * ActionSystem.Instance.SuccessCount;
        var targets = FieldManager.Instance.Targets;

        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].TakeDamage(power);
        }

        _anim.SetInteger(ATTACK_INTEGER_PARAM, (int)_attackType);
        OnFinish();
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