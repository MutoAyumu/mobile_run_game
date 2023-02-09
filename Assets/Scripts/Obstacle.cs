using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : ArrangementObject, IDamage
{
    [SerializeField] float _power = 1f;
    [SerializeField] float _health = 1f;

    float _currentHealth;
    MeshRenderer _mesh;
    Collider _collider;

    private void Awake()
    {
        TryGetComponent(out _mesh);
        TryGetComponent(out _collider);
    }

    public void TakeDamage(float damage)
    {
        if (!IsActive) return;

        _currentHealth -= damage;

        if(_currentHealth <= 0)
        {
            Destroy();
        }
    }
    private void OnEnable()
    {
        FieldManager.Instance.Targets.Add(this);
    }
    private void OnDisable()
    {
        FieldManager.Instance.Targets.Remove(this);
    }

    protected override void OnHit(GameObject go)
    {
        var player = go.GetComponent<IDamage>();
        player.TakeDamage(_power);
        Destroy();
    }

    public override void Create()
    {
        base.Create();
        _mesh.enabled = IsActive;
        _collider.enabled = IsActive;
        _currentHealth = _health;
    }

    public override void Destroy()
    {
        base.Destroy();
        _mesh.enabled = IsActive;
        _collider.enabled = IsActive;
    }

    public override void DisactiveForInstantiate()
    {
        base.DisactiveForInstantiate();
        _mesh.enabled = IsActive;
        _collider.enabled = IsActive;
    }
}
