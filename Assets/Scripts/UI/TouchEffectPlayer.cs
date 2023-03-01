using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.InputSystem.LowLevel;

public class TouchEffectPlayer : MonoBehaviour
{
    [SerializeField] Image _touchEffect;

    bool _isActive;

    private void Awake()
    {
        _touchEffect.enabled = false;

#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isRemoteConnected)
        {
            InputSystemManager.Instance.TouchState.Subscribe(IsProgress).AddTo(this);
        }
        else
        {
            InputSystemManager.Instance.EditorTouchButton.Subscribe(_ => IsProgress()).AddTo(this);
            InputSystemManager.Instance.EditorTouchPoint.Subscribe(IsEffectMove).AddTo(this);
        }
#endif
#if UNITY_ANDROID
        InputSystemManager.Instance.TouchState.Subscribe(IsProgress).AddTo(this);
#endif
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
    void IsProgress()
    {
        _isActive = !_isActive;

        _touchEffect.enabled = _isActive;
    }
    void IsEffectMove(Vector2 vec)
    {
        _touchEffect.transform.position = vec;
    }
}
