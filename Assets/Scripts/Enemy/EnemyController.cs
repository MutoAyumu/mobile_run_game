using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyController : MonoBehaviour
{
    EnemyHealth _health;

    private void Awake()
    {
        TryGetComponent(out _health);

        _health.Init(this);
    }
}
