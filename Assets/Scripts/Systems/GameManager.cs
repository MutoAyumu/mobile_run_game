using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.InputSystem;

public class GameManager
{
    #region �ϐ�
    ReactiveProperty<float> _gameTime = new ReactiveProperty<float>();
    List<IDamage> _targets = new List<IDamage>();
    bool _isPause;
    readonly GameInputs _inputs = new GameInputs();
    readonly Subject<Unit> _pauseSubject = new Subject<Unit>();
    #endregion

    #region �v���p�e�B
    public static GameManager Instance => new GameManager();
    public IObservable<Unit> OnPauseSubject => _pauseSubject;
    public IReadOnlyReactiveProperty<float> GameTime => _gameTime;
    public List<IDamage> Targets => _targets;
    #endregion

    public GameManager()
    {
        _inputs.Player.PauseResume.started += OnPause;
        _inputs.Enable();
    }

    public void Init(GameManagerAttachment attachment)
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

        Debug.Log("Update");
        _gameTime.Value -= Time.deltaTime;
    }
    void OnPause(InputAction.CallbackContext context)
    {
        _isPause = !_isPause;
        _pauseSubject.OnNext(Unit.Default);
    }
}