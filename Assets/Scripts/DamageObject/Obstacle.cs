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
    Transform _thisTransform;
    LayerMask _playerLayer;
    IDamageObject _data;
    DamageObject _damageObject;

    const string PLAYER_LAYER = "Player";

    private void Awake()
    {
        TryGetComponent(out _thisTransform);

        _playerLayer = LayerMask.GetMask(PLAYER_LAYER);
    }

    public void Init(DamageObject damage)
    {
        var pos = _thisTransform.position;
        damage.SetCenterPosition(pos);
        _damageObject = damage;

        _data = damage.Data;
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
            _data.Action(_damageObject.transform);
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
        _currentHealth = _health;
        _isIntrusion = false;
    }

    public override void Destroy()
    {
        base.Destroy();
        _data = null;
    }

    public override void DisactiveForInstantiate()
    {
        base.DisactiveForInstantiate();
    }
    private void OnDrawGizmosSelected()
    {
        var t = Application.isPlaying ? _thisTransform.position : this.transform.position;
        var center = t + _areaCenter;
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(center, _areaSize);
    }
}
