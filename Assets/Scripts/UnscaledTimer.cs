using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnscaledTimer : TimerBase
{
    public override bool RunTimer()
    {
        _timer += Time.unscaledDeltaTime;

        if (_timer >= _interval)
        {
            _timer = 0;
            return true;
        }

        return false;
    }
}
