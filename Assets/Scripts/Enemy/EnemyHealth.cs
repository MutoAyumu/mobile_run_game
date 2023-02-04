using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyHealth : MonoBehaviour , IDamage
{
    [SerializeField] FloatReactiveProperty _health = new FloatReactiveProperty(10);

    public IReadOnlyReactiveProperty<float> Health => _health;

    public void Init(EnemyController enemy)
    {

    }
    public void TakeDamage(float damage)
    {
        _health.Value -= damage;
    }
}
