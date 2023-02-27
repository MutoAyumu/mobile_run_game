using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageObject
{
    public int CreateCount { get; }
    public float Spacing { get; }
    public float SpacingFromPrevData { get; }
    public Mesh Mesh { get; }
    public Material Material { get; }
    public GenerationPosition[] Positions { get; }

    public void Init();

    public void Action();
}
public enum GenerationPosition
{
    Right,
    Center,
    Left
}
