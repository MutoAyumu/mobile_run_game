using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.InputSystem.LowLevel;

public class ActionSystem
{
    #region 変数
    ActionData _currentData;
    bool _isCompleted = true;
    Subject<Unit> _actionSub = new Subject<Unit>();
    Subject<Unit> _isCompletedSub = new Subject<Unit>();
    ReactiveProperty<float> _skillInterval = new ReactiveProperty<float>();

    readonly static ActionSystem _instance = new ActionSystem();
    #endregion

    #region プロパティ
    public static ActionSystem Instance => _instance;
    public IObservable<Unit> IsCompleted => _isCompletedSub;
    public IObservable<Unit> Action => _actionSub;
    public IReadOnlyReactiveProperty<float> SkillInterval => _skillInterval;
    public int SuccessCount => _currentData.State.SuccessCount;
    public float SkillIntervalData => _currentData.State.SkillInterval;
    #endregion

    public ActionSystem() 
    {
        Debug.Log("New ActionSystem");
    }

    public void Init(ActionSystemAttachment attachment)
    {
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                Update();
            }).AddTo(attachment);

        InputSystemManager.Instance.ActionSub.Subscribe(_ => StartedAction()).AddTo(attachment);

#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isRemoteConnected)
        {
            InputSystemManager.Instance.TouchState.Subscribe(TappedTouchState).AddTo(attachment);
        }
        else
        {
            InputSystemManager.Instance.EditorTouchButton.Subscribe(_ => AddTapCount(1)).AddTo(attachment);
        }
#endif
#if UNITY_ANDROID
        InputSystemManager.Instance.TouchState.Subscribe(TappedTouchState).AddTo(attachment);
#endif

        _skillInterval.Value = 0;
        _isCompleted = true;
    }

    public void OnStart(ActionData data)
    {
        _currentData = data;
        _currentData.State.Enter();
    }

    public void ChangeActionData(ActionData data)
    {
        _currentData.State.Exit();
        _currentData = data;
        _currentData.State.Enter();
    }

    void Update()
    {
        if (_isCompleted)
        {
            if (_currentData.State.SkillInterval > _skillInterval.Value)
            {
                _skillInterval.Value += Time.deltaTime;
            }
            return;
        }

        var com = _currentData.State.Update();

        if (com == true)
        {
            _isCompleted = true;
            _isCompletedSub.OnNext(Unit.Default);
            CameraManager.Instance.ChangePreferredOrder(VCameraType.PlayerFollow);
            CameraManager.Instance.ChangeTimeScale(TimeScaleType.NormalTime);
        }
    }

    void AddTapCount(int value)
    {
        //加速のタップはここをそのまま使っていくかも
        Debug.Log($"タップカウント : ");
    }
    void TappedTouchState(TouchState state)
    {
        if (state.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            AddTapCount(1);
        }
    }

    void StartedAction()
    {
        if (!_isCompleted) return;

        if (_skillInterval.Value < _currentData.State.SkillInterval) return;

        _isCompleted = false;
        _skillInterval.Value = 0;
        _actionSub.OnNext(Unit.Default);
        _currentData.State.Init();
        CameraManager.Instance.ChangePreferredOrder(VCameraType.Action);
        CameraManager.Instance.ChangeTimeScale(TimeScaleType.SlowTime);
    }
}
