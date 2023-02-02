using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(ObservablePointerEnterTrigger))]
public class TestAttackAction : MonoBehaviour
{
    ObservablePointerEnterTrigger _enterTrigger;

    private void Awake()
    {
        TryGetComponent(out _enterTrigger);
        _enterTrigger.OnPointerEnterAsObservable()
            .Subscribe(_ => Debug.Log("EnterTrigger"));
    }
}
