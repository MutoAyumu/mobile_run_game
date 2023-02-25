using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour, IObjectPool
{
    bool _isActive;
    MeshRenderer _mesh;
    Collider _collider;

    public bool IsActive => _isActive;

    private void Awake()
    {
        TryGetComponent(out _mesh);
        TryGetComponent(out _collider);
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
