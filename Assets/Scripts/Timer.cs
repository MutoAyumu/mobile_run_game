using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : TimerBase
{
    public override bool RunTimer()
    {
        _timer += Time.deltaTime;

        if (_timer >= _interval)
        {
            _timer = 0;
            return true;
        }

        return false;
    }
}
