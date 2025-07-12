using System.Collections.Generic;

public class GenericPoolManager : MonoSingleton<GenericPoolManager>
{
    Dictionary<PoolingObject, Pool<PoolingObject>> _pools = new();

    public PoolingObject GetPoolingObject(PoolingObject poolingObject)
    {
        if (!_pools.ContainsKey(poolingObject))
        {
            Pool<PoolingObject> pool = new Pool<PoolingObject>(poolingObject);

            _pools.Add(poolingObject, pool);
        }

        return _pools[poolingObject].Get();

    }
}