using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour, IObjectPool
{
    bool _isActive;
    MeshRenderer _mesh;
    MeshFilter _filter;
    Collider _collider;

    public bool IsActive => _isActive;

    private void Awake()
    {
        TryGetComponent(out _mesh);
        TryGetComponent(out _filter);
        TryGetComponent(out _collider);
    }

    public void SetData(IDamageObject data)
    {
        _filter.mesh = data.Mesh;
        _mesh.material = data.Material;
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
