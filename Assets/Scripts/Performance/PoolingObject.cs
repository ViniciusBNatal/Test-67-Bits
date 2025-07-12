using System;
using UnityEngine;

public class PoolingObject : MonoBehaviour
{
    [SerializeField] private PoolObjectData _poolData;

    public Action<PoolingObject> OnFinished;
    //public PoolObjectData PoolData => _poolData; 
    protected virtual void OnDisable()
    {
        OnFinished?.Invoke(this);
    }
}
