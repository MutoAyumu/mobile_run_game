using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public abstract class DamageObjectData : IDamageObject
{
    [Header("CommonParameter")]
    [SerializeField] float _spacing = 5f;
    [SerializeField] float _spacingFromPrevData = 20f;
    [SerializeField] Mesh _mesh;
    [SerializeField] Material _material;
    [SerializeField] Vector3 _modelRotation;
    [SerializeField] float _dissolveSpeed = 0.5f;

    protected int _createCount = 1;
    GenerationPositionData[] _positionsData;

    public int CreateCount => _createCount;
    public float Spacing => _spacing;
    public float SpacingFromPrevData => _spacingFromPrevData;
    public Mesh Mesh => _mesh;
    public Material Material => _material;
    public Vector3 Rotation => _modelRotation;
    public GenerationPositionData[] GenerationPositionData => _positionsData;

    public DamageObjectData() { }

    const string DISSOLVE_PARAM = "_Cutoff";

    protected void SetCreateCountData(int num)
    {
        _createCount = num;
    }

    protected void SetPositionsData(GenerationPositionData[] datas)
    {
        _positionsData = datas;
    }

    protected void DissolveFade(Tween tween, MeshRenderer renderer)
    {
        DOVirtual.Float(1, 0, _dissolveSpeed, value => renderer.materials[0].SetFloat(DISSOLVE_PARAM, value))
            .OnComplete(() =>
            {
                tween.Play();
            });
    }

    public abstract void Init();
    public abstract void Action(Transform t, MeshRenderer renderer);
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

    public override void Action(Transform t, MeshRenderer renderer)
    {
        var endValue = t.position.y + _moveValue;
        var tween = t.DOMoveY(endValue, _moveSpeed)
            .SetEase(_ease);

        DissolveFade(tween, renderer);
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

    public override void Action(Transform t, MeshRenderer renderer)
    {
        throw new System.NotImplementedException();
    }
}
