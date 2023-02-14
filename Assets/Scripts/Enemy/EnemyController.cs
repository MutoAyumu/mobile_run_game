using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public partial class EnemyController : MonoBehaviour//, IDamage
{
    //#region 変数
    //[Header("Health")]
    //[SerializeField] FloatReactiveProperty _health = new FloatReactiveProperty(10);

    //Animator _anim;
    //Transform _thisTransform;
    //StatePatternBase<EnemyController> _statePattern;    

    //const string TAKEDAMAGE_PARAM = "IsTakeDamage";
    //const string DEATH_PARAM = "IsDead";
    //#endregion

    //#region プロパティ
    //public IReadOnlyReactiveProperty<float> Health => _health;
    //#endregion

    //private void Awake()
    //{
    //    Init();
    //}
    //private void Start()
    //{
    //    _statePattern.OnStart((int)StateType.Idle);
    //}

    //void Init()
    //{
    //    _thisTransform = this.transform;
    //    TryGetComponent(out _generator);
    //    TryGetComponent(out _anim);

    //    _statePattern = new StatePatternBase<EnemyController>(this);
    //    _statePattern.Add<EnemyDeadState>((int)StateType.Dead);
    //    _statePattern.Add<EnemyIdleState>((int)StateType.Idle);
    //    _statePattern.Add<EnemyAttackState>((int)StateType.Attack);
    //}

    //private void OnEnable()
    //{
    //    FieldManager.Instance.Targets.Add(this);
    //}

    //private void OnDisable()
    //{
    //    FieldManager.Instance.Targets.Remove(this);
    //}

    //private void Update()
    //{
    //    _statePattern.OnUpdate();
    //}

    //public void TakeDamage(float damage)
    //{
    //    if (_statePattern.CheckCurrentStateID((int)StateType.Dead) is null or true) return;

    //    _health.Value -= damage;

    //    if (_health.Value <= 0)
    //    {
    //        //_statePattern.ChangeState((int)StateType.Dead);
    //        _anim.SetTrigger(DEATH_PARAM);
    //        return;
    //    }

    //    _anim.SetTrigger(TAKEDAMAGE_PARAM);
    //}
    //public enum StateType
    //{
    //    Dead = -1,
    //    Idle = 0,
    //    Attack = 1,
    //}
}