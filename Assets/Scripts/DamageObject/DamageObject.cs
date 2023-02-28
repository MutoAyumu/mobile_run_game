using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour, IObjectPool
{
    bool _isActive;
    IDamageObject _data;
    float _verticalPosition;
    MeshRenderer _mesh;
    MeshFilter _filter;
    Collider _collider;

    public bool IsActive => _isActive;

    public IDamageObject Data => _data;

    private void Awake()
    {
        TryGetComponent(out _mesh);
        TryGetComponent(out _filter);
        TryGetComponent(out _collider);
    }

    public void SetData(IDamageObject data, int index)
    {
        _data = data;
        _filter.mesh = data.Mesh;
        _mesh.material = data.Material;
        _verticalPosition = data.GenerationPositionData[index].VerticalPosition;
    }

    public void SetCenterPosition(Vector3 center)
    {
        SetVerticalPosition(center);
    }

    void SetVerticalPosition(Vector3 center)
    {
        var pos = center;
        pos.y = _verticalPosition;
        this.transform.position = pos;
    }

    public void Create()
    {
        _isActive = true;
        _collider.enabled = _isActive;
        _mesh.enabled = _isActive;
    }

    public void Destroy()
    {
        _isActive = false;
        _collider.enabled = _isActive;
        _mesh.enabled = _isActive;
    }

    public void DisactiveForInstantiate()
    {
        _isActive = false;
        _collider.enabled = _isActive;
        _mesh.enabled = _isActive;
    }
}
