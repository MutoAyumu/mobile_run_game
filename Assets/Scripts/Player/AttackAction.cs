using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(ObservablePointerEnterTrigger))]
public class AttackAction : MonoBehaviour
{
    [SerializeField] float _sizeChangeSpeed = 1f;
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] float _addActionPower = 1f;
    [SerializeField] Ease _ease = Ease.Linear;

    ObservablePointerEnterTrigger _enterTrigger;
    Transform _thisTransform;
    Image _image;

    private void Awake()
    {
        TryGetComponent(out _enterTrigger);
        TryGetComponent(out _thisTransform);
        TryGetComponent(out _image);

        SizeChangeAnim(Vector2.one, null);
    }
    public void Init(System.Action<float> action)
    {
        _enterTrigger.OnPointerEnterAsObservable().Subscribe(_ =>
        {
            _image.raycastTarget = false;
            action?.Invoke(_addActionPower);
            SizeChangeAnim(Vector2.zero, () => Destroy(gameObject));
        }).AddTo(this);
    }
    void SizeChangeAnim(Vector2 size, System.Action action)
    {
        _thisTransform.DOScale(size, _sizeChangeSpeed)
            .SetEase(_ease)
            .OnComplete(() =>
            {
                action?.Invoke();
            });
    }

    public void Create(Vector2 orizin, Vector2 dir)
    {
        _thisTransform.transform.position = orizin;
        _thisTransform.DOMove(dir, _moveSpeed);
    }
}
