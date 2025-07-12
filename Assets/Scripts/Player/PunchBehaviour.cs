using System;
using UnityEngine;
using UnityEngine.Events;

public class PunchBehaviour : MonoBehaviour
{
    [SerializeField] private UnityEvent _onPunch;

    public Action<Vector3> OnPunch;
    private void OnTriggerEnter(Collider other)
    {
        OnPunch?.Invoke(other.transform.position);
        _onPunch?.Invoke();
    }
}
