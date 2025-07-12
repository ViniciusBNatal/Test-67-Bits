using UnityEngine;
[CreateAssetMenu(menuName = "67Bits/ObjectPooling/PoolObjectData", fileName = "NewPoolObjectData")]
public class PoolObjectData : ScriptableObject
{
    [SerializeField, PoolType] private int _poolType;
    [SerializeField] private PoolingObject _poolObject;

    public int PoolType => _poolType;
    public PoolingObject PoolObject => _poolObject;
}
