using UnityEngine;

public class SpawnPileObject : MonoBehaviour
{
    [SerializeField] private Vector3[] _spawnPositionOffsets;
    [SerializeField] private PoolObjectData[] _spawnDatas;
#if UNITY_EDITOR
    [SerializeField] private Color _spawnPositionGizmoColor = Color.red;
#endif

    public void Spawn()
    {
        PoolingObject obj = GenericPoolManager.Instance.GetPoolingObject(_spawnDatas[Random.Range(0, _spawnDatas.Length)]);
        obj.transform.position = transform.position + _spawnPositionOffsets[Random.Range(0, _spawnPositionOffsets.Length)];
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_spawnPositionOffsets == null) return;
        Gizmos.color = _spawnPositionGizmoColor;
        for (int i = 0; i < _spawnPositionOffsets.Length; i++)
        {
            UnityEditor.Handles.Label(transform.position + _spawnPositionOffsets[i] + Vector3.up, new GUIContent(i.ToString()));
            Gizmos.DrawSphere(transform.position + _spawnPositionOffsets[i], .1f);
        }
    }
#endif
}