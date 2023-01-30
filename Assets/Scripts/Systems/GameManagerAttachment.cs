using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManagerAttachment : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.Init(this);
    }
    private void Start()
    {
        GameManager.Instance.OnPauseSubject.Subscribe(_ => Debug.Log("Pause")).AddTo(this);
    }
}
