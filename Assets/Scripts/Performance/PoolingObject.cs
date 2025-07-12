using System;
using UnityEngine;

public class PoolingObject : MonoBehaviour
{
    public Action<PoolingObject> OnFinished;

    protected virtual void OnDisable()
    {
        OnFinished?.Invoke(this);
    }
}
