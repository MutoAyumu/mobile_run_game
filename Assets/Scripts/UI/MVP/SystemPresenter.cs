using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SystemPresenter : MonoBehaviour
{
    [SerializeField] MVPText _gameTimeText;
    [SerializeField] MVPText _scoreText;
    [SerializeField] string _timeTextFormat = "00:00";
    [SerializeField] string _scoreTextFormat = "000000";

    private void Awake()
    {
        if(_gameTimeText)
        {
            FieldManager.Instance.GameTime.Subscribe(x =>
            {
                _gameTimeText.SetText($"{x.ToString(_timeTextFormat)}");
            }).AddTo(this);
        }
        if(_scoreText)
        {
            FieldManager.Instance.Score.Subscribe(x =>
            {
                _scoreText.SetText($"{x.ToString(_scoreTextFormat)}");
            }).AddTo(this);
        }
    }
}
