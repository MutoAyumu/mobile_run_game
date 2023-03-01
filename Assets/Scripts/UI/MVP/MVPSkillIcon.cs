using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class MVPSkillIcon : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] float _animSpeed = 0.2f;

    Tween _tween;

    private void Reset()
    {
        TryGetComponent(out _image);
    }

    public void SetFillAmount(float value)
    {
        if (_tween != null)
            _tween.Kill();

        var from = _image.fillAmount;
        var to = value;

        _tween = DOVirtual.Float(from, to, _animSpeed, v => _image.fillAmount = v);

        Debug.Log(to);
    }
}
