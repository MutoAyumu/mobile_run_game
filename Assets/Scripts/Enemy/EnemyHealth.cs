using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyHealth : MonoBehaviour , IDamage
{
    [SerializeField] FloatReactiveProperty _health = new FloatReactiveProperty(10);
    Animator _anim;
    EnemyController _enemy;

    const string TAKEDAMAGE_PARAM = "IsTakeDamage";
    const string DEATH_PARAM = "IsDead";

    public IReadOnlyReactiveProperty<float> Health => _health;

    public void Init(EnemyController enemy)
    {
        _enemy = enemy;
        TryGetComponent(out _anim);
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
        if (_enemy.CurrentState == LifeState.Dead) return;

        _health.Value -= damage;

        if(_health.Value <= 0)
        {
            _enemy.CurrentState = LifeState.Dead;
            _anim.SetTrigger(DEATH_PARAM);
            return;
        }

        _anim.SetTrigger(TAKEDAMAGE_PARAM);
    }
}
