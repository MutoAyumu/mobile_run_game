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
    ReactiveProperty<int> _tapCount = new ReactiveProperty<int>();
    
    readonly static ActionSystem _instance = new ActionSystem();
    #endregion

    #region プロパティ
    public static ActionSystem Instance => _instance;
    public IObservable<Unit> IsCompleted => _isCompletedSub;
    public IObservable<Unit> Action => _actionSub;
    public IReadOnlyReactiveProperty<int> TapCount => _tapCount;
    public int SuccessCount => _currentData.State.SuccessCount;
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
        if (_isCompleted) return;

        var com = _currentData.State.Update();

        if (com == true)
        {
            _isCompleted = true;
            _isCompletedSub.OnNext(Unit.Default);
            CameraManager.Instance.ChangePreferredOrder(VCameraType.PlayerFollow);
            CameraManager.Instance.ChangeTimeScale(TimeScaleType.NormalTime);

            Debug.Log($"残りのタップカウント : {_tapCount}");
        }
    }

    void AddTapCount(int value)
    {
        _tapCount.Value += value;

        Debug.Log($"タップカウント : {_tapCount}");
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

        if (_tapCount.Value < _currentData.State.RequiredTapCount) return;

        _isCompleted = false;
        _tapCount.Value = 0;
        _actionSub.OnNext(Unit.Default);
        _currentData.State.Init();
        CameraManager.Instance.ChangePreferredOrder(VCameraType.Action);
        CameraManager.Instance.ChangeTimeScale(TimeScaleType.SlowTime);
    }
}
