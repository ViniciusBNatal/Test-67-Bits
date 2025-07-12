using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : PoolingObject
{
    private Queue<PoolingObject> m_Pool;
    private PoolingObject m_Prefab;

    public Pool(PoolObjectData prefab)
    {
        m_Pool = new Queue<PoolingObject>();

        m_Prefab = prefab.PoolObject;
    }

    public T Get()
    {
        PoolingObject feedback;
        if (m_Pool.Count > 0)
        {
            feedback = m_Pool.Dequeue();
            feedback.gameObject.SetActive(true);
            return feedback as T;
        }

        feedback = Object.Instantiate(m_Prefab);
        feedback.OnFinished += ReturnToPool;
        return feedback as T;
    }

    private void ReturnToPool(PoolingObject obj)
    {
        m_Pool.Enqueue(obj);
    }
}