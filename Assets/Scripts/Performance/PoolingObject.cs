using System;
using UnityEngine;

public class PoolingObject : MonoBehaviour
{
    [SerializeField] private PoolObjectData _poolData;

    public Action<PoolingObject> OnFinished;
    protected virtual void OnDisable()
    {
        OnFinished?.Invoke(this);
    }
}
