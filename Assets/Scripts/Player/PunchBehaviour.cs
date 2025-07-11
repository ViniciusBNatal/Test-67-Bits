using System;
using UnityEngine;

public class PunchBehaviour : MonoBehaviour
{
    public Action<Vector3> OnPunch;

    private void OnTriggerEnter(Collider other)
    {        
        OnPunch?.Invoke(other.transform.position);
    }
}
