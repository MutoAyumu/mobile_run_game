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
    private void OnEnable()
    {
        FieldManager.Instance.Targets.Add(this);
        Debug.Log(FieldManager.Instance.Targets.Count);
    }
    private void OnDisable()
    {
        FieldManager.Instance.Targets.Remove(this);
    }
    public void TakeDamage(float damage)
    {
        _health.Value -= damage;
        Debug.Log($"TakeDamage {damage}");
    }
}
