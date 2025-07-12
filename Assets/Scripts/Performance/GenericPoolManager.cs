using System.Collections.Generic;

public class GenericPoolManager : MonoSingleton<GenericPoolManager>
{
    Dictionary<int, Pool<PoolingObject>> _pools = new();

    public PoolingObject GetPoolingObject(PoolObjectData poolObjectData)
    {
        if (!_pools.ContainsKey(poolObjectData.PoolType))
        {
            Pool<PoolingObject> pool = new Pool<PoolingObject>(poolObjectData);

            _pools.Add(poolObjectData.PoolType, pool);
        }

        return _pools[poolObjectData.PoolType].Get();

    }
}