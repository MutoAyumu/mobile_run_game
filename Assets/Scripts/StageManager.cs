using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] GameObject _prefab = default;

    List<GameObject> _maps = new List<GameObject>();
    [SerializeField] int _startMaps = 3;
    [SerializeField] int _mapWidth = 14;
    [SerializeField] float _speed = 2f;

    private void Start()
    {
        Create();
    }
    void Create()
    {
        _maps.Add(Instantiate(_prefab));
        _maps.Add(Instantiate(_prefab, new Vector3(0, 0, _mapWidth), Quaternion.identity));
        _maps.Add(Instantiate(_prefab, new Vector3(0, 0, _mapWidth + _mapWidth), Quaternion.identity));
    }
    public void CreateStage()
    {
        int createIndex = 2 * _mapWidth;
        Vector3 mapsWidth = new Vector3(0, 0, createIndex);

        _maps[_startMaps % _maps.Count].transform.position = mapsWidth;
        _startMaps++;
    }
    private void Update()
    {
        foreach(var m in _maps)
        {
            var t = m.transform;
            t.Translate(0, 0, Time.deltaTime * -_speed);
        }
        if(_maps[_startMaps % _maps.Count].transform.position.z <= -_mapWidth)
        {
            CreateStage();
        }
    }
}