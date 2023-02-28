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
    public Vector3 Rotation { get; }
    public GenerationPositionData[] GenerationPositionData { get; }

    public void Init();

    public void Action(Transform t, MeshRenderer renderer);
}

public enum GenerationPosition
{
    Right,
    Center,
    Left
}
