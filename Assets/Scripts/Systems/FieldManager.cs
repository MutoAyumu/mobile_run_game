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
    ReactiveProperty<float> _score = new ReactiveProperty<float>();
    List<IDamage> _targets = new List<IDamage>();
    bool _isPause;
    float _scoreMultiplicationValue;

    readonly GameInputs _inputs = new GameInputs();
    readonly Subject<Unit> _pauseSubject = new Subject<Unit>();
    readonly static FieldManager _instance = new FieldManager();
    #endregion
    #region　プロパティ
    public IReadOnlyReactiveProperty<float> GameTime => _gameTime;
    public IReadOnlyReactiveProperty<float> Score => _score;
    public List<IDamage> Targets => _targets;
    public IObservable<Unit> OnPauseSub => _pauseSubject;
    #endregion

    public static FieldManager Instance = _instance;
    private FieldManager()
    {
        _inputs.Player.PauseResume.started += OnPause;
        _inputs.Enable();

        Debug.Log("New FieldManager");
    }

    public void Init(FieldManagerAttachment attachment)
    {
        _gameTime.Value = attachment.GameTime;
        _score.Value = 0;
        _scoreMultiplicationValue = attachment.ScoreMultiplicationValue;

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
        _score.Value += Time.deltaTime * _scoreMultiplicationValue;
    }

    public void AddScore(int score)
    {
        _score.Value += score;
    }

    void OnPause(InputAction.CallbackContext context)
    {
        _isPause = !_isPause;
        _pauseSubject.OnNext(Unit.Default);
    }
}
