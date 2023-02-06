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
            GameManager.Instance.GameTime.Subscribe(x =>
            {
                _gameTimeText.SetText(x.ToString());
            }).AddTo(this);
        }
    }
}
