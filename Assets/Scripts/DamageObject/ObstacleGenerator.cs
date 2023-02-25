using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    #region 変数
    [Header("Obstacle")]
    [SerializeField] int _obstacleCreateLimit = 20;
    [SerializeField] Obstacle _obstaclePrefab;
    [SerializeField] string _obstacleRootName = "ObstacleRoot";
    [SerializeField] Transform _obstacleParent;
    GenericObjectPool<Obstacle> _obstaclPool = new GenericObjectPool<Obstacle>();
    [Header("DamageObject")]
    [SerializeField] int _damageObjectCreateLimit = 20;
    [SerializeField] DamageObject _damageObjectPrefab;
    [SerializeField] string _damageObjectRootName = "DamageObjectRoot";
    [SerializeField] Transform _damageObjectParent;
    GenericObjectPool<DamageObject> _damageObjectPool = new GenericObjectPool<DamageObject>();
    #endregion

    #region プロパティ

    #endregion

    private void Start()
    {
        SetPool(_obstacleRootName, _obstacleParent, _obstaclPool, _obstaclePrefab, _obstacleCreateLimit);
        SetPool(_damageObjectRootName, _damageObjectParent, _damageObjectPool, _damageObjectPrefab, _damageObjectCreateLimit);
    }

    void SetPool<T>(string rootName, Transform parent, GenericObjectPool<T> pool, IObjectPool prefab, int limit)
        where T : UnityEngine.Object, IObjectPool
    {
        var root = new GameObject(rootName).transform;

        if (parent)
        {
            root.SetParent(parent);
        }

        pool.SetBaseObj((T)prefab, root);
        pool.SetCapacity(limit);
    }
    public void OnObstacleCreate()
    {
        var obs = _obstaclPool.Instantiate();
        var dmo = _damageObjectPool.Instantiate();
        obs.Init(dmo);
    }
}
