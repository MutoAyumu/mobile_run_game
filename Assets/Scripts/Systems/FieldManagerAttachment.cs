using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class FieldManagerAttachment : MonoBehaviour
{
    [SerializeField] float _gameTime = 60f;
    [SerializeField, Range(1,100)] float _scoreMultiplicationValue = 1;

    public float GameTime => _gameTime;
    public float ScoreMultiplicationValue => _scoreMultiplicationValue;

    private void Awake()
    {
        FieldManager.Instance.Init(this);
    }
}
