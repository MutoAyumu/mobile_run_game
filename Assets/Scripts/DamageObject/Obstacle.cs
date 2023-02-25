using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : ArrangementObject
{
    [SerializeField] float _power = 1f;
    [SerializeField] float _health = 1f;
    [SerializeField] Vector3 _areaSize = Vector3.one;
    [SerializeField] Vector3 _areaCenter = Vector3.zero;

    float _currentHealth;
    bool _isIntrusion;
    MeshRenderer _mesh;
    Collider _collider;
    Transform _thisTransform;
    LayerMask _playerLayer;
    DamageObject _damageObject;

    const string PLAYER_LAYER = "Player";

    private void Awake()
    {
        TryGetComponent(out _mesh);
        TryGetComponent(out _collider);
        TryGetComponent(out _thisTransform);

        _playerLayer = LayerMask.GetMask(PLAYER_LAYER);
    }

    public void Init(DamageObject damage)
    {
        _damageObject = damage;
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

    private void Update()
    {
        if (_isIntrusion || !IsActive) return;

        var center = _areaCenter + _thisTransform.position;
        var hit = Physics.OverlapBox(center, _areaSize, Quaternion.identity, _playerLayer);

        if(hit.Length > 0)
        {
            _isIntrusion = true;
            Debug.Log("PlayerÇ∆ê⁄êGÇµÇΩ");
        }
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
        _isIntrusion = false;
    }

    public override void Destroy()
    {
        base.Destroy();
        _mesh.enabled = IsActive;
        _collider.enabled = IsActive;
        _damageObject = null;
    }

    public override void DisactiveForInstantiate()
    {
        base.DisactiveForInstantiate();
        _mesh.enabled = IsActive;
        _collider.enabled = IsActive;
    }
    private void OnDrawGizmosSelected()
    {
        var t = Application.isPlaying ? _thisTransform.position : this.transform.position;
        var center = t + _areaCenter;
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(center, _areaSize);
    }
}
