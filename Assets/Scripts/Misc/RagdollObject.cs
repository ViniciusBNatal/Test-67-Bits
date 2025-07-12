using UnityEngine;

public class RagdollObject : PoolingObject
{
    private Rigidbody[] _rigidbodies;
    private Collider[] _colliders;

    private void Setup()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();
    }

    public void UpdatePhysics(bool isActive)
    {
        if (_rigidbodies == null) Setup();
        for (int i = 0; i < _rigidbodies.Length; i++)
        {
            _rigidbodies[i].isKinematic = isActive;
        }
        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].enabled = isActive;
        }
    } 
}
