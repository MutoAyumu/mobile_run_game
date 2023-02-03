using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UniRx;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Image _touchEffect;
    [SerializeField] AttackAction _action;

    Camera _cam;
    int _tapCount;
    Transform _actionParent;
    Transform[] _actionOrizinPositions;

    const string ACTION_PARENT_TAG = "ActionParent";
    const string ACTION_OBJECT_POSITION = "ActionObjPos";

    public void Init(PlayerController player)
    {
        player.Input.TouchState.Subscribe(x => IsInProgress(x)).AddTo(this);
        player.Input.ActionSub.Subscribe(_ => OnAction()).AddTo(this);
        _touchEffect.enabled = false;

        _cam = Camera.main;
        _actionParent = GameObject.FindGameObjectWithTag(ACTION_PARENT_TAG).transform;
        var objects = GameObject.FindGameObjectsWithTag(ACTION_OBJECT_POSITION);
        _actionOrizinPositions = Array.ConvertAll(objects, go => go.transform);
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
        var list = new List<Transform>(_actionOrizinPositions);

        for(int i = 0; i < _actionOrizinPositions.Length; i++)
        {
            var r = UnityEngine.Random.Range(0, list.Count);
            var obj = Instantiate(_action, list[r].position, Quaternion.identity, _actionParent);
            obj.name = $"Action[{r}]";
            SubscribeAction(obj);
            list.RemoveAt(r);
        }
    }
}
