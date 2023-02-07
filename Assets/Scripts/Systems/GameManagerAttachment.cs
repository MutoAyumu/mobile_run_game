using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class GameManagerAttachment : MonoBehaviour
{
    [SerializeField] float _gameTime = 60f;

    public delegate void MonoEvent();
    MonoEvent _updateEvent;

    public float GameTime => _gameTime;

    private void Awake()
    {
        GameManager.Instance.Init(this);
    }
    private void Start()
    {
        GameManager.Instance.OnPauseSubject.Subscribe(_ => Debug.Log("Pause")).AddTo(this);
    }
    private void Update()
    {
        _updateEvent?.Invoke();
    }
    public void Init(MonoEvent e)
    {
        _updateEvent = e;
    }
}
