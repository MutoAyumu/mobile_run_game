using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UniRx;

[System.Serializable]
public class EnemyAttackState// : IState
{
    //#region �ϐ�
    //[Header("Attack")]
    //[SerializeField] Transform[] _setPositions;
    //[SerializeField] LayerMask _layer;
    //ArrangementObjectGenerator _generator;
    //#endregion

    //#region �v���p�e�B
    //#endregion

    //const string ATTACK_PARAM = "IsAttack";
    //const string ATTACK_ANIMATION_TAG = "Attack";

    //public void Init()
    //{
    //    SetAnimationTrigger();
    //}

    //public void OnEnter()
    //{
    //    Owner._anim.SetTrigger(ATTACK_PARAM);
    //}
    //public int OnUpdate()
    //{
    //    throw new System.NotImplementedException();
    //}

    //public void OnExit()
    //{
    //    throw new System.NotImplementedException();
    //}

    //void SetAnimationTrigger()
    //{
    //    var trigger = Owner._anim.GetBehaviour<ObservableStateMachineTrigger>();

    //    trigger
    //        .OnStateExitAsObservable()
    //        .Subscribe(stateInfo =>
    //        {
    //            var info = stateInfo.StateInfo;

    //            if (info.IsTag(ATTACK_ANIMATION_TAG))
    //            {
    //                OnAttack();
    //            }
    //        }).AddTo(Owner);
    //}
    //void OnAttack()
    //{
    //    if (Owner._statePattern.CheckCurrentStateID((int)StateType.Dead) is null or true) return;

    //    var r = Random.Range(0, Owner._setPositions.Length);
    //    var obj = Owner._generator.OnCreate();
    //    obj.position = Owner._setPositions[r].position;
    //    obj.SetParent(CheckParent());

    //    //�����������炱�̃^�C�~���O�Ŏ��ґh�������\������
    //    StatePattern.ChangeState((int)StateType.Idle);
    //}
    //Transform CheckParent()
    //{
    //    RaycastHit hit;
    //    Physics.Raycast(Owner._thisTransform.position, Vector3.down, out hit, 3, Owner._layer);
    //    return hit.transform;
    //}
}