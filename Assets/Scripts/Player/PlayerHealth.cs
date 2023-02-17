using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerHealth : MonoBehaviour, IDamage
{
    #region �ϐ�
    [Header("Parameter")]
    [SerializeField] ReactiveProperty<float> _health = new ReactiveProperty<float>(3);

    Animator _anim;
    #endregion

    #region �v���p�e�B
    public IReadOnlyReactiveProperty<float> Health => _health;
    #endregion

    #region �萔
    const string TAKEDAMAGE_PARAM = "IsTakeDamage";
    #endregion

    private void Awake()
    {
        TryGetComponent(out _anim);
    }

    public void TakeDamage(float damage)
    {
        _health.Value -= damage;
        _anim.SetTrigger(TAKEDAMAGE_PARAM);
    }
}