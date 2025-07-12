using UnityEngine;

[CreateAssetMenu(menuName = "67Bits/ObjectPooling/PoolTypeList", fileName = "NewPoolTypeList")]
public class PoolTypeList : ScriptableObject
{
    [SerializeField] private string[] _poolTypes;
    public string[] PoolTypes => _poolTypes;
}
