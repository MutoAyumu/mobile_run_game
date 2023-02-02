using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UniRx;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Image _touchEffect;
    [SerializeField] AttackAction _action;
    [SerializeField] Transform[] _testPos;
    Camera _cam;
    int _tapCount;

    public void Init(PlayerController player)
    {
        player.Input.TouchState.Subscribe(x => IsInProgress(x)).AddTo(this);
        player.Input.ActionSub.Subscribe(_ => OnAction()).AddTo(this);
        _touchEffect.enabled = false;
        _cam = Camera.main;
    }
    void IsInProgress(TouchState state)
    {
        if(state.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            _touchEffect.enabled = true;
            _tapCount++;
        }
        else if(state.phase == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            _touchEffect.enabled = false;
        }

        if(state.isInProgress)
        {
            _touchEffect.transform.position = state.position;
        }
    }
    void SubscribeAction(AttackAction action)
    {
        action.Init(() => Debug.Log("Attack"));
    }
    void OnAction()
    {
        _tapCount -= 10;
        var r = Random.Range(0, _testPos.Length);
        //ランダムな位置にアクションオブジェクトを生成
    }
}
