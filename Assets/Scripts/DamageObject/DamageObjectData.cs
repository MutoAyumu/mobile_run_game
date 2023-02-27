using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageObjectData : IDamageObject
{
    [Header("CommonParameter")]
    [SerializeField] float _spacing = 5f;
    [SerializeField] float _spacingFromPrevData = 20f;
    [SerializeField] Mesh _mesh;
    [SerializeField] Material _material;

    protected int _createCount = 1;
    GenerationPosition[] _positions;

    public int CreateCount => _createCount;
    public float Spacing => _spacing;
    public float SpacingFromPrevData => _spacingFromPrevData;
    public Mesh Mesh => _mesh;
    public Material Material => _material;
    public GenerationPosition[] Positions => _positions;

    public DamageObjectData() { }

    protected void SetCreateCountData(int num)
    {
        _createCount = num;
    }
    
    protected void SetPositionsData(GenerationPosition[] positions)
    {
        _positions = positions;
    }

    public abstract void Init();
    public abstract void Action();
}

public class Spike : DamageObjectData
{
    [Header("Spike")]
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] GenerationPosition[] _positions = new GenerationPosition[1];

    public Spike()
    {
        Debug.Log(typeof(Spike));
    }

    public override void Init()
    {
        SetCreateCountData(_positions.Length);
        SetPositionsData(_positions);
    }

    public override void Action()
    {
        throw new System.NotImplementedException();
    }
}
public class Bullet : DamageObjectData
{
    [Header("Bullet")]
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] GenerationPosition[] _positions = new GenerationPosition[1];

    public Bullet()
    {
        Debug.Log(typeof(Bullet));
    }

    public override void Init()
    {
        SetCreateCountData(_positions.Length);
        SetPositionsData(_positions);
    }

    public override void Action()
    {
        throw new System.NotImplementedException();
    }
}
