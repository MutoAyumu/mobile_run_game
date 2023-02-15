using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.InputSystem.LowLevel;

public class TouchEffectPlayer : MonoBehaviour
{
    [SerializeField] Image _touchEffect;

    private void Awake()
    {
        _touchEffect.enabled = false;
        InputSystemManager.Instance.TouchState.Subscribe(IsProgress).AddTo(this);
    }

    void IsProgress(TouchState state)
    {
        if (state.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            _touchEffect.enabled = true;
        }
        else if (state.phase == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            _touchEffect.enabled = false;
        }

        if (state.isInProgress)
        {
            _touchEffect.transform.position = state.position;
        }
    }
}
