using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrangementObjectGenerator : MonoBehaviour
{
    #region 変数
    [SerializeField] int _createLimit = 20;
    [SerializeField] ArrangementObject _prefab;
    [SerializeField] string _name = "Root";
    [SerializeField] Transform _parent;
    GenericObjectPool<ArrangementObject> _pool = new GenericObjectPool<ArrangementObject>();
    #endregion

    #region プロパティ

    #endregion

    private void Start()
    {
        var root = new GameObject(_name).transform;

        if(_parent)
        {
            root.SetParent(_parent);
        }

        _pool.SetBaseObj(_prefab, root);
        _pool.SetCapacity(_createLimit);
    }
    public Transform OnCreate()
    {
        var obj = _pool.Instantiate();
        return obj.transform;
    }
}
