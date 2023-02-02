using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(ObservablePointerEnterTrigger))]
public class AttackAction : MonoBehaviour
{
    ObservablePointerEnterTrigger _enterTrigger;

    private void Awake()
    {
        TryGetComponent(out _enterTrigger);
    }
    public void Init(System.Action action)
    {
        _enterTrigger.OnPointerEnterAsObservable().Subscribe(_ => action());
    }
}
