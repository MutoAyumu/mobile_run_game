using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.InputSystem;
using System;

public class FieldManager
{
    #region　変数
    ReactiveProperty<float> _gameTime = new ReactiveProperty<float>();
    List<IDamage> _targets = new List<IDamage>();
    bool _isPause;

    readonly GameInputs _inputs = new GameInputs();
    readonly Subject<Unit> _pauseSubject = new Subject<Unit>();
    #endregion
    #region　プロパティ
    public IReadOnlyReactiveProperty<float> GameTime => _gameTime;
    public List<IDamage> Targets => _targets;
    public IObservable<Unit> OnPauseSub => _pauseSubject;
    #endregion

    public static FieldManager Instance = new FieldManager();
    private FieldManager()
    {
        _inputs.Player.PauseResume.started += OnPause;
        _inputs.Enable();
    }

    public void Init(FieldManagerAttachment attachment)
    {
        _gameTime.Value = attachment.GameTime;

        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                Update();
            }).AddTo(attachment);
    }

    void Update()
    {
        if (_isPause) return;

        _gameTime.Value -= Time.deltaTime;
    }

    void OnPause(InputAction.CallbackContext context)
    {
        _isPause = !_isPause;
        _pauseSubject.OnNext(Unit.Default);
    }
}
