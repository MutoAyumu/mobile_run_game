using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class FieldManagerAttachment : MonoBehaviour
{
    [SerializeField] float _gameTime = 60f;

    public float GameTime => _gameTime;

    private void Awake()
    {
        FieldManager.Instance.Init(this);
    }
}
