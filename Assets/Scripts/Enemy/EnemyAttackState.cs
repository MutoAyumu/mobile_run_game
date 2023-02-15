using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UniRx;

[System.Serializable]
public class EnemyAttackState : IState
{
    #region 変数
    [SerializeField] Transform[] _setPositions;
    [SerializeField] LayerMask _layer;

    ArrangementObjectGenerator _generator;
    GameObject _owner;
    Animator _anim;
    Transform _thisTransform;
    bool _isAttack;
    #endregion

    #region プロパティ
    #endregion

    #region 定数
    const string ATTACK_PARAM = "IsAttack";
    const string ATTACK_ANIMATION_TAG = "Attack";
    const string OWNER_TAG = "Enemy";
    #endregion

    public void Init()
    {
        _owner = GameObject.FindGameObjectWithTag(OWNER_TAG);
        _owner.TryGetComponent(out _anim);
        _owner.TryGetComponent(out _thisTransform);
        _owner.TryGetComponent(out _generator);

        SetAnimationTrigger();
    }

    public void OnEnter()
    {
        _anim.SetTrigger(ATTACK_PARAM);
    }
    public int OnUpdate()
    {
        if(_isAttack)
        {
            return (int)EnemyController.StateType.Idle;
        }

        return (int)EnemyController.StateType.Attack;
    }

    public void OnExit()
    {
        _isAttack = false;
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
                    OnAttack();
                    _isAttack = true;
                }
            }).AddTo(_owner);
    }
    void OnAttack()
    {
        var r = Random.Range(0, _setPositions.Length);
        var obj = _generator.OnCreate();
        obj.position = _setPositions[r].position;
        obj.SetParent(CheckParent());
    }
    Transform CheckParent()
    {
        RaycastHit hit;
        Physics.Raycast(_thisTransform.position, Vector3.down, out hit, 3, _layer);
        return hit.transform;
    }
}