using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Toolkit;

public class GenericObjectPool<T> where T : UnityEngine.Object, IObjectPool
{
    T _baseObj = null;
    Transform _parent = null;
    List<T> _pool = new List<T>();
    int _index = 0;

    public List<T> PoolList => _pool;
    
    public void SetBaseObj(T obj, Transform parent)
    {
        _baseObj = obj;
        _parent = parent;
    }
    /// <summary>
    /// PoolListに追加
    /// </summary>
    /// <param name="obj"></param>
    public void Pooling(T obj)
    {
        obj.DisactiveForInstantiate();
        _pool.Add(obj);
    }
    /// <summary>
    /// Poolのサイズを設定してPoolingする
    /// </summary>
    /// <param name="size"></param>
    public void SetCapacity(int size)
    {
        _pool.Clear();

        //既にオブジェクトサイズが大きいときは更新しない
        if (size < _pool.Count) return;

        for (int i = 0; i < size; i++)
        {
            T obj = default(T);
            if (_parent)
            {
                obj = GameObject.Instantiate(_baseObj, _parent);
            }
            else
            {
                obj = GameObject.Instantiate(_baseObj);
            }
            Pooling(obj);
        }

        Debug.Log($"<color=cyan>{this._baseObj.name}</color> : {size}個生成終了");
    }
    /// <summary>
    /// Poolされているオブジェクトを取り出す
    /// </summary>
    /// <returns></returns>
    public T Instantiate()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            int index = i;
            if (_pool[index].IsActive) continue;

            _pool[index].Create();
            return _pool[index];
        }

        T obj = default(T);
        if (_parent)
        {
            obj = GameObject.Instantiate(_baseObj, _parent);
        }
        else
        {
            obj = GameObject.Instantiate(_baseObj);
        }
        Pooling(obj);
        obj.Create();
        return obj;
    }
}
