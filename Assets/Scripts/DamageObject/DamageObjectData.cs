using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class DamageObjectData : IDamageObject
{
    [Header("CommonParameter")]
    [SerializeField] float _spacing = 5f;
    [SerializeField] float _spacingFromPrevData = 20f;
    [SerializeField] Mesh _mesh;
    [SerializeField] Material _material;

    protected int _createCount = 1;
    GenerationPositionData[] _positionsData;

    public int CreateCount => _createCount;
    public float Spacing => _spacing;
    public float SpacingFromPrevData => _spacingFromPrevData;
    public Mesh Mesh => _mesh;
    public Material Material => _material;
    public GenerationPositionData[] GenerationPositionData => _positionsData;

    public DamageObjectData() { }

    protected void SetCreateCountData(int num)
    {
        _createCount = num;
    }
    
    protected void SetPositionsData(GenerationPositionData[] datas)
    {
        _positionsData = datas;
    }

    public abstract void Init();
    public abstract void Action(Transform t);
}

[System.Serializable]
public class GenerationPositionData
{
    [SerializeField] GenerationPosition _position;
    [SerializeField] float _beginVerticalPosition = 1;

    public GenerationPosition Position => _position;
    public float VerticalPosition => _beginVerticalPosition;
}

public class Spike : DamageObjectData
{
    [Header("Spike")]
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] Ease _ease;
    [SerializeField] float _moveValue = 1f;
    [SerializeField] GenerationPositionData[] _positionsData = new GenerationPositionData[1];

    public Spike()
    {
        Debug.Log(typeof(Spike));
    }

    public override void Init()
    {
        SetCreateCountData(_positionsData.Length);
        SetPositionsData(_positionsData);
    }

    public override void Action(Transform t)
    {
        var endValue = t.position.y + _moveValue;
        t.DOMoveY(endValue, _moveSpeed)
            .SetEase(_ease);
    }
}
public class Bullet : DamageObjectData
{
    [Header("Bullet")]
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] GenerationPositionData[] _positionsData = new GenerationPositionData[1];

    public Bullet()
    {
        Debug.Log(typeof(Bullet));
    }

    public override void Init()
    {
        SetCreateCountData(_positionsData.Length);
        SetPositionsData(_positionsData);
    }

    public override void Action(Transform t)
    {
        throw new System.NotImplementedException();
    }
}
