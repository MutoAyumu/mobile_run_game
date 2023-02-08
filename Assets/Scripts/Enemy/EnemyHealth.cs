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
        GameManager.Instance.Targets.Add(this);
        Debug.Log(GameManager.Instance.Targets.Count);
    }
    private void OnDisable()
    {
        GameManager.Instance.Targets.Remove(this);
    }
    public void TakeDamage(float damage)
    {
        _health.Value -= damage;
        Debug.Log($"TakeDamage {damage}");
    }
}
