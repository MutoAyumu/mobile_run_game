using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ValueModel
{
    public ReactiveProperty<float> CurrentValue { get; private set; }

    public ValueModel(float value)
    {
        CurrentValue = new ReactiveProperty<float>(value);
    }
    
    public void AddValue(float value)
    {
        CurrentValue.Value += value;
    }

    public void ResetValue()
    {
        CurrentValue.Value = 0;
    }
}
