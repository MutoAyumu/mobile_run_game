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
    [SerializeField] Transform _center;

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
        ActionSystem.Instance.IsCompleted.Subscribe(_ =>
        { 
            OnAttack();
        });

        ActionSystem.Instance.OnStart(_actionData);

        SetAnimationTrigger();
    }

    public void Init(PlayerController owner)
    {
        _owner = owner;
    }

    void OnAttack()
    {
        var power = _power * ActionSystem.Instance.SuccessCount;
        var targets = FieldManager.Instance.Targets;

        var skill = Instantiate(_actionData.State.Skill, _center.position, Quaternion.identity);
        skill.SetDamageValue(power);

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