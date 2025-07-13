using UnityEngine;

public class RagdollObject : PoolingObject
{
    [SerializeField] private PoolObjectData _pileObjectToAdd;

    private Rigidbody[] _rigidbodies;
    private Collider[] _colliders;

    protected override void OnDisable()
    {
        base.OnDisable();
        if(PileManager.Instance) PileManager.Instance.AddToPile(_pileObjectToAdd);
    }

    private void Setup()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();
    }

    public void UpdatePhysics(bool isActive)
    {
        if (_rigidbodies == null) Setup();
        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].enabled = isActive;
        }
        for (int i = 0; i < _rigidbodies.Length; i++)
        {
            _rigidbodies[i].isKinematic = !isActive;
        }
    }
}
