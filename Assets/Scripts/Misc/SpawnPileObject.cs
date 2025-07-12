using UnityEngine;

public class SpawnPileObject : MonoBehaviour
{
    [SerializeField] private Vector3 _spawnPositionOffset;
    [SerializeField] private PoolObjectData _spawnData;
#if UNITY_EDITOR
    [SerializeField] private Color _spawnPositionGizmoColor = Color.red;
#endif

    public void Spawn()
    {
        PoolingObject obj = GenericPoolManager.Instance.GetPoolingObject(_spawnData);
        obj.transform.position = transform.position + _spawnPositionOffset;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _spawnPositionGizmoColor;
        Gizmos.DrawSphere(transform.position + _spawnPositionOffset, .1f);
    }
#endif
}