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
    [SerializeField] Ease _ease = Ease.Linear;

    ObservablePointerEnterTrigger _enterTrigger;
    IDisposable _disposable;
    RectTransform _thisTransform;
    Tweener _moveTween;
    bool _isActive;
    float _destroyTime;
    Action _timeOutEvent;

    public bool IsActive => _isActive;

    private void Awake()
    {
        TryGetComponent(out _enterTrigger);
        TryGetComponent(out _thisTransform);

        SizeChangeAnim(Vector2.zero, 0, null);
    }
    public void Init(Action pointerEnterEvent, Action timeOutEvent)
    {
        _disposable = _enterTrigger.OnPointerEnterAsObservable().Subscribe(_ =>
        {
            _disposable.Dispose();
            pointerEnterEvent?.Invoke();
            _moveTween.Kill();
            SizeChangeAnim(Vector2.zero, _sizeChangeSpeed, () => Destroy());
        }).AddTo(this);

        _timeOutEvent = timeOutEvent;
    }
    void SizeChangeAnim(Vector2 size, float speed, System.Action action)
    {
        _thisTransform.DOScale(size, speed)
            .SetEase(_ease)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                action?.Invoke();
            });
    }

    public void Setup(Vector2 orizin, Vector2 dir, float destroyTime, float moveSpeed)
    {
        _thisTransform.transform.position = orizin;
        _destroyTime = destroyTime;

        _moveTween = _thisTransform.DOAnchorPos(dir, moveSpeed)
            .SetUpdate(true)
            .SetEase(_ease)
            .SetRelative(true)
            .OnComplete(() =>
            {
                SizeChangeAnim(Vector2.zero, _sizeChangeSpeed, () =>
                {
                    _disposable.Dispose();
                    _timeOutEvent?.Invoke();
                    Destroy();
                });
            });
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
