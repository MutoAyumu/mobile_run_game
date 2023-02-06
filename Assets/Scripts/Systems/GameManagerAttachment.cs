using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class GameManagerAttachment : MonoBehaviour
{
    public delegate void MonoEvent();
    MonoEvent _updateEvent;

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
