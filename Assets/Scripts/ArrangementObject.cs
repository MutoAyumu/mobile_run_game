using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArrangementObject : MonoBehaviour, IObjectPool
{
    const string PLAYER_TAG = "Player";
    const string DESTROY_TAG = "Finish";
    bool _isActive;

    public bool IsActive => _isActive;

    public virtual void Create() => _isActive = true;

    public virtual void Destroy() => _isActive = false;

    public virtual void DisactiveForInstantiate() => _isActive = false;

    protected abstract void OnHit(GameObject go);

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
            OnHit(other.gameObject);
        if (other.gameObject.CompareTag(DESTROY_TAG))
            Destroy();
    }
}