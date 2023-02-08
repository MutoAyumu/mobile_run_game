using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class GameManagerAttachment : MonoBehaviour
{
    [SerializeField] float _gameTime = 60f;

    public float GameTime => _gameTime;

    private void Awake()
    {
        GameManager.Instance.Init(this);
    }
}
