using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerHealth : MonoBehaviour, IDamage
{
    #region 変数
    [SerializeField] ReactiveProperty<float> _health = new ReactiveProperty<float>(3);

    Animator _anim;
    #endregion

    #region プロパティ
    public IReadOnlyReactiveProperty<float> Health => _health;
    #endregion

    const string TAKEDAMAGE_PARAM = "IsTakeDamage";

    public void Init(PlayerController player)
    {
        TryGetComponent(out _anim);
    }

    public void TakeDamage(float damage)
    {
        _health.Value -= damage;
        _anim.SetTrigger(TAKEDAMAGE_PARAM);
    }
}
