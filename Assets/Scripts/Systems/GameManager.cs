using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.InputSystem;

public class GameManager
{
    #region ïœêî
    ReactiveProperty<float> _gameTime = new ReactiveProperty<float>();
    GameInputs _inputs = new GameInputs();
    List<IDamage> _targets = new List<IDamage>();
    readonly Subject<Unit> _pauseSubject = new Subject<Unit>();
    #endregion

    public static GameManager Instance => new GameManager();
    public IObservable<Unit> OnPauseSubject => _pauseSubject;
    public IReadOnlyReactiveProperty<float> GameTime => _gameTime;
    public List<IDamage> Targets => _targets;
    public GameManager()
    {
        _inputs.Player.PauseResume.performed += OnPause;
        _inputs.Enable();
    }

    public void Init(GameManagerAttachment attachment)
    {
        attachment.Init(Update);
    }
    void Update()
    {
        Debug.Log("Update");
    }
    void OnPause(InputAction.CallbackContext context)
    {
        _pauseSubject.OnNext(Unit.Default);
    }
}
