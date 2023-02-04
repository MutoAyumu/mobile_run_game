using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimerBase
{
    protected float _timer;
    protected float _interval = 1f;

    public void Setup(float time)
    {
        _interval = time;
    }
    public abstract bool RunTimer();
}
