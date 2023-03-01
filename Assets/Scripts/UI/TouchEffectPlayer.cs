using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.InputSystem.LowLevel;

public class TouchEffectPlayer : MonoBehaviour
{
    [SerializeField] ParticleSystem _tapEffectPrefab;
    [SerializeField] TrailRenderer _holdEffectPrefab;
    [SerializeField] Camera _uiCamera;
    [SerializeField] float _cameraZ = 5f;

    ParticleSystem _tapEffect;
    TrailRenderer _holdEffect;
    Vector2 _tapPoint;

    private void Awake()
    {
        _tapEffect = Instantiate(_tapEffectPrefab);
        _holdEffect = Instantiate(_holdEffectPrefab);

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
            _tapEffectPrefab.Play();
            MoveEffect(_tapEffect.transform, state.position);
        }

        if (state.isInProgress)
        {
            MoveEffect(_holdEffect.transform, state.position);
        }
    }
    void IsProgress()
    {
        _tapEffect.Play();
        MoveEffect(_tapEffect.transform, _tapPoint);
    }
    void IsEffectMove(Vector2 vec)
    {
        _tapPoint = vec;
        MoveEffect(_holdEffect.transform, vec);
    }

    void MoveEffect(Transform t, Vector3 pos)
    {
        var vec = pos;
        vec.z = _cameraZ;

        t.position = _uiCamera.ScreenToWorldPoint(vec);
    }
}
