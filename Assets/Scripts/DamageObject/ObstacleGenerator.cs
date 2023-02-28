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

    int _currentDataIndex;
    int _createCount;
    float _spacing;
    Vector3[] _generationPosition;
    IDamageObject[] _obstacleDatas;
    IDamageObject _currentObstacleData;
    #endregion

    #region プロパティ

    #endregion

    public void SetTrackData(TrackData data)
    {
        _obstacleDatas = data.ObstacleDataArray;
        _currentObstacleData = _obstacleDatas[_currentDataIndex];

        _spacing += _currentObstacleData.SpacingFromPrevData;

        foreach(var d in _obstacleDatas)
        {
            d.Init();
        }
    }
    public void SetPositionData(Vector3[] positions)
    {
        _generationPosition = positions;
    }

    private void Start()
    {
        SetPool(_obstacleRootName, _obstacleParent, _obstaclPool, _obstaclePrefab, _obstacleCreateLimit);
        SetPool(_damageObjectRootName, _damageObjectParent, _damageObjectPool, _damageObjectPrefab, _damageObjectCreateLimit);

        for(int i = 0; i < _obstacleDatas.Length; i++)
        {
            for(int j = 0; j < _obstacleDatas[i].GenerationPositionData.Length; j++)
            {
                OnObstacleCreate();
            }
        }
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
    void OnObstacleCreate()
    {
        var obs = _obstaclPool.Instantiate();
        var dmo = _damageObjectPool.Instantiate();

        dmo.SetData(_obstacleDatas[_currentDataIndex], _createCount);
        SetPosition(obs, _currentObstacleData.GenerationPositionData[_createCount].Position);
        obs.Init(dmo);

        _createCount++;

        if (_createCount >= _currentObstacleData.CreateCount)
        {
            if (_currentDataIndex + 1 < _obstacleDatas.Length)
            {
                _currentDataIndex++;
                _createCount = 0;
                _currentObstacleData = _obstacleDatas[_currentDataIndex];
                _spacing += _currentObstacleData.SpacingFromPrevData;
            }
        }
    }

    void SetPosition(Obstacle obj, GenerationPosition pos)
    {
        var p = Vector3.zero;

        switch(pos)
        {
            case GenerationPosition.Right:
                p = _generationPosition[0];
                break;
            case GenerationPosition.Center:
                p = _generationPosition[1];
                break;
            case GenerationPosition.Left:
                p = _generationPosition[2];
                break;
        }

        p.z = _spacing;
        obj.transform.position = p;
        _spacing += _currentObstacleData.Spacing;
    }
}
