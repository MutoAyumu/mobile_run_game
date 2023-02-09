using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SystemPresenter : MonoBehaviour
{
    [SerializeField] MVPText _gameTimeText;

    private void Awake()
    {
        if(_gameTimeText)
        {
            FieldManager.Instance.GameTime.Subscribe(x =>
            {
                _gameTimeText.SetText($"GameTime : {x:00}");
            }).AddTo(this);
        }
    }
}
