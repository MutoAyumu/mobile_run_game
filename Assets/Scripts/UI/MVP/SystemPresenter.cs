using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SystemPresenter : MonoBehaviour
{
    [SerializeField] MVPText _gameTimeText;
    [SerializeField] MVPText _scoreText;
    [SerializeField] string _format = "0000";

    private void Awake()
    {
        if(_gameTimeText)
        {
            FieldManager.Instance.GameTime.Subscribe(x =>
            {
                _gameTimeText.SetText($"GameTime : {x.ToString(_format)}");
            }).AddTo(this);
        }
        if(_scoreText)
        {
            FieldManager.Instance.Score.Subscribe(x =>
            {
                _scoreText.SetText($"Score : {x.ToString(_format)}");
            }).AddTo(this);
        }
    }
}
