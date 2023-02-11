using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UniRx;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UniRx.Triggers;
using StateBase = StatePatternBase<PlayerController>.StateBase;

public partial class PlayerController
{
    [SerializeField] AttackAction _action;
    [SerializeField] AttackType _attackType = AttackType.First;

    public class PlayerActionState : StateBase
    {
        [SerializeField] Image _touchEffect;

        Camera _cam;
        int _tapCount;
        int _actionObjectCount;
        float _attackPower;
        Transform _actionParent;
        Transform[] _actionOrizinPositions;
        ReactiveProperty<bool> _isActed = new ReactiveProperty<bool>();

        public IReadOnlyReactiveProperty<bool> IsActed => _isActed;

        const string ACTION_PARENT_TAG = "ActionParent";
        const string ACTION_OBJECT_POSITION = "ActionObjPos";
        const string ATTACK_ANIMATION_TAG = "Attack";
        const string ATTACK_INTEGER_PARAM = "AttackNumber";

        public override void Init(PlayerController owner)
        {
            owner._input.TouchState.Subscribe(x => IsInProgress(x)).AddTo(Owner);
            //_touchEffect.enabled = false;

            _cam = Camera.main;
            SetAnimationTrigger();
            _actionParent = GameObject.FindGameObjectWithTag(ACTION_PARENT_TAG).transform;
            var objects = GameObject.FindGameObjectsWithTag(ACTION_OBJECT_POSITION);
            _actionOrizinPositions = Array.ConvertAll(objects, go => go.transform);
        }

        public override void OnEnter()
        {
            OnAction();
            _tapCount -= 10;
            _isActed.Value = true;
            CameraManager.Instance.ChangePreferredOrder(VCameraType.Action);
            CameraManager.Instance.ChangeTimeScale(TimeScaleType.SlowTime);
        }

        public override void OnUpdate()
        {
            if (_actionObjectCount >= _actionOrizinPositions.Length)
            {
                OnAttack(_attackPower);
                StatePattern.ChangeState((int)StateType.Move);
            }
        }

        public override void OnExit()
        {
            _isActed.Value = false;
            _actionObjectCount = 0;
            _attackPower = 0;
            CameraManager.Instance.ChangePreferredOrder(VCameraType.PlayerFollow);
            CameraManager.Instance.ChangeTimeScale(TimeScaleType.NormalTime);
        }

        void IsInProgress(TouchState state)
        {
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
                var obj = Instantiate(Owner._action, list[r].position, Quaternion.identity, _actionParent);
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

            Owner._anim.SetInteger(ATTACK_INTEGER_PARAM, (int)Owner._attackType);
        }
        void SetAnimationTrigger()
        {
            var trigger = Owner._anim.GetBehaviour<ObservableStateMachineTrigger>();

            trigger
                .OnStateExitAsObservable()
                .Subscribe(stateInfo =>
                {
                    var info = stateInfo.StateInfo;

                    if (info.IsTag(ATTACK_ANIMATION_TAG))
                    {
                        Owner._anim.SetInteger(ATTACK_INTEGER_PARAM, (int)AttackType.None);
                    }
                }).AddTo(Owner);
        }
    }
    enum AttackType
    {
        None = 0,
        First = 1,
        Second = 2,
        Third = 3
    }
}
