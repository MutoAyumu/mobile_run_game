using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TimePresenter : MonoBehaviour
{
    [SerializeField] TimeView _view;
    [SerializeField] ValueModel _model;
    [SerializeField] float _initTime = 60f;

    private void Awake()
    {
        _model = new ValueModel(_initTime);
        _view.Init();
        _view.SetText(_initTime.ToString());

        Bind();
    }

    void Bind()
    {
        _model.CurrentValue.Subscribe(ChangeText).AddTo(this);
    }

    void ChangeText(float value)
    {
        _view.SetText(value.ToString());
    }

    public void AddValue(float value)
    {
        _model.AddValue(value);
    }
}
