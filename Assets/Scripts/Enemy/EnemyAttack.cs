using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(ArrangementObjectGenerator))]
public class EnemyAttack : MonoBehaviour
{
    #region 変数
    [SerializeField] float _power = 1f;
    [SerializeField] float _interval = 2f;
    [SerializeField] Transform[] _setPositions;
    ArrangementObjectGenerator _generator;
    [SerializeField] LayerMask _layer;

    Timer _attackTimer = new Timer();
    Animator _anim;
    EnemyController _enemy;
    #endregion

    const string ATTACK_PARAM = "IsAttack";
    const string ATTACK_ANIMATION_TAG = "Attack";

    #region プロパティ
    #endregion

    public void Init(EnemyController enemy)
    {
        enemy.OnUpdateSub.Subscribe(_ => OnUpdate()).AddTo(this);

        _attackTimer.Setup(_interval);
        _enemy = enemy;
        TryGetComponent(out _generator);
        TryGetComponent(out _anim);
        SetAnimationTrigger();
    }
    void OnUpdate()
    {
        if (_enemy.CurrentState == LifeState.Dead) return;

        if(_attackTimer.RunTimer())
        {
            _anim.SetTrigger(ATTACK_PARAM);
        }
    }
    void OnAttack()
    {
        var r = Random.Range(0, _setPositions.Length);
        var obj = _generator.OnCreate();
        obj.position = _setPositions[r].position;
        obj.SetParent(CheckParent());
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
                }
            }).AddTo(this);
    }
    Transform CheckParent()
    {
        RaycastHit hit;
        Physics.Raycast(this.transform.position, Vector3.down, out hit, 3, _layer);
        return hit.transform;
    }
}
