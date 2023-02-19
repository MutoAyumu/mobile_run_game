using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(ObservablePointerEnterTrigger))]
public class AttackAction : MonoBehaviour, IObjectPool
{
    [SerializeField] float _sizeChangeSpeed = 1f;
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] float _destroyTime = 2.5f;
    [SerializeField] Ease _ease = Ease.Linear;

    ObservablePointerEnterTrigger _enterTrigger;
    IDisposable _disposable;
    Transform _thisTransform;
    Vector2 _vec;
    UnscaledTimer _timer = new UnscaledTimer();
    bool _isActive;
    Action _timeOutEvent;

    public bool IsActive => _isActive;

    private void Awake()
    {
        TryGetComponent(out _enterTrigger);
        TryGetComponent(out _thisTransform);

        SizeChangeAnim(Vector2.zero, 0 ,null);
        _timer.Setup(_destroyTime);
    }
    public void Init(Action pointerEnterEvent, Action timeOutEvent)
    {
        _disposable = _enterTrigger.OnPointerEnterAsObservable().Subscribe(_ =>
        {
            _disposable.Dispose();
            pointerEnterEvent?.Invoke();
            SizeChangeAnim(Vector2.zero, _sizeChangeSpeed, () => Destroy());
        }).AddTo(this);

        _timeOutEvent = timeOutEvent;
    }
    void SizeChangeAnim(Vector2 size, float speed, System.Action action)
    {
        _thisTransform.DOScale(size, speed)
            .SetEase(_ease)
            .OnComplete(() =>
            {
                action?.Invoke();
            });
    }

    public void Setup(Vector2 orizin, Vector2 dir)
    {
        _thisTransform.transform.position = orizin;
        _vec = dir;
    }

    private void Update()
    {
        if(_timer.RunTimer())
        {
            SizeChangeAnim(Vector2.zero, _sizeChangeSpeed, () =>
            {
                _disposable.Dispose();
                _timeOutEvent?.Invoke();
                Destroy();
            });
        }

        _thisTransform.Translate(_vec * _moveSpeed * Time.deltaTime);
    }

    public void DisactiveForInstantiate()
    {
        _isActive = false;
    }

    public void Create()
    {
        _isActive = true;
        SizeChangeAnim(Vector2.one, _sizeChangeSpeed, null);
    }

    public void Destroy()
    {
        _isActive = false;
    }
}
