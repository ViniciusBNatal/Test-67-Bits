using UnityEngine;

public class UpdateRagdoll : MonoBehaviour
{
    private Rigidbody[] _rigidbodies;
    private Collider[] _colliders;
    [SerializeField] private bool _isActive;

    private void Awake()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();
        UpdatePhysics();
    }

    [ContextMenu("Update")]
    private void UpdatePhysics()
    {
        for (int i = 0; i < _rigidbodies.Length; i++)
        {
            _rigidbodies[i].isKinematic = !_isActive;
        }
        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].enabled = _isActive;
        }
    }
}
