using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

[RequireComponent(typeof(ObservablePointerEnterTrigger))]
public class AttackAction : MonoBehaviour
{
    [SerializeField] float _animSpeed = 1f;
    [SerializeField] float _addActionPower = 1f;
    [SerializeField] Ease _ease = Ease.Linear;

    ObservablePointerEnterTrigger _enterTrigger;
    Transform _thisTransform;

    private void Awake()
    {
        TryGetComponent(out _enterTrigger);
        TryGetComponent(out _thisTransform);

        Animation(Vector2.one, null);
    }
    public void Init(System.Action<float> action)
    {
        _enterTrigger.OnPointerEnterAsObservable().Subscribe(_ =>
        {
            action?.Invoke(_addActionPower);
            Animation(Vector2.zero, () => Destroy(gameObject));
        }).AddTo(this);
    }
    void Animation(Vector2 size, System.Action action)
    {
        _thisTransform.DOScale(size, _animSpeed)
            .SetEase(_ease)
            .OnComplete(() =>
            {
                action?.Invoke();
            });
    }
}
