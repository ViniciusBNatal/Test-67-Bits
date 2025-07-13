using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : PoolingObject
{
    private Queue<PoolingObject> _pool;
    private PoolingObject _prefab;

    public Pool(PoolObjectData prefab)
    {
        _pool = new Queue<PoolingObject>();

        _prefab = prefab.PoolObject;
    }

    public T Get()
    {
        PoolingObject feedback;
        if (_pool.Count > 0)
        {
            feedback = _pool.Dequeue();
            feedback.gameObject.SetActive(true);
            return feedback as T;
        }

        feedback = Object.Instantiate(_prefab);
        feedback.OnFinished += ReturnToPool;
        return feedback as T;
    }

    private void ReturnToPool(PoolingObject obj)
    {
        _pool.Enqueue(obj);
    }
}