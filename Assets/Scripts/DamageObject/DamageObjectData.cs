using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObjectData
{
    [Header("CommonParameter")]
    [SerializeField] protected int _createCount = 1;
    [SerializeField] protected float _spacing = 5f;

    public int CreateCount => _createCount;
    public float Spacing => _spacing;
}

public class Spike : DamageObjectData, IDamageObject
{
    [Header("Spike")]
    [SerializeField] float _moveSpeed = 1f;

    public Spike()
    {
        Debug.Log(typeof(Spike));
    }

    public void Start()
    {

    }
}
public class Bullet : DamageObjectData, IDamageObject
{
    [Header("Bullet")]
    [SerializeField] float _moveSpeed = 1f;

    public Bullet()
    {
        Debug.Log(typeof(Bullet));
    }

    public void Start()
    {

    }
}
