using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UniRx;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Image _touchEffect;
    Camera _cam;

    public void Init(PlayerController player)
    {
        player.Input.TouchState.Subscribe(x => IsInProgress(x)).AddTo(this);
        _touchEffect.enabled = false;
        _cam = Camera.main;
    }
    void IsInProgress(TouchState state)
    {
        if(state.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            _touchEffect.enabled = true;
        }
        else if(state.phase == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            _touchEffect.enabled = false;
        }

        if(state.isInProgress)
        {
            _touchEffect.transform.position = state.position;
            IsAttack(state.position);
        }
    }
    void IsAttack(Vector2 pos)
    {
        
    }
}
