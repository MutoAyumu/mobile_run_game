using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.InputSystem;

public class GameManager
{
    GameInputs _inputs = new GameInputs();

    public static GameManager Instance => new GameManager();

    public GameManager() 
    {
        _inputs.Player.PauseResume.performed += OnPause;
        _inputs.Enable();
    }

    readonly Subject<Unit> _pauseSubject = new Subject<Unit>();
    public IObservable<Unit> OnPauseSubject => _pauseSubject;

    public void Init(GameManagerAttachment attachment)
    {
        
    }
    void Update()
    {

    }
    void OnPause(InputAction.CallbackContext context)
    {
        _pauseSubject.OnNext(Unit.Default);
    }
}
