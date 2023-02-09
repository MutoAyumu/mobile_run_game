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
        enemy.OnEnableSub.Subscribe(_ => Enable()).AddTo(this);
        enemy.OnDisableSub.Subscribe(_ => Disable()).AddTo(this);
    }
    private void Enable()
    {
        FieldManager.Instance.Targets.Add(this);
    }
    private void Disable()
    {
        FieldManager.Instance.Targets.Remove(this);
    }
    public void TakeDamage(float damage)
    {
        _health.Value -= damage;
    }
}
