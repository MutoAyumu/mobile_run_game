using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject : MonoBehaviour
{
    protected float _damageValue;

    public void SetDamageValue(float value)
    {
        _damageValue = value;
    }
}
