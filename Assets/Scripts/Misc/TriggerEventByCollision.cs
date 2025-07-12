using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerEventByCollision : MonoBehaviour
{
    [SerializeField] private TriggerTypes _triggerType;
    [SerializeField] private UnityEvent _onExecute;
    private enum TriggerTypes
    {
        OnTriggerEnter,
        OnTriggerExit,
        OnCollisionEnter,
        OnCollisionExit
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!this.enabled) return;
        if (_triggerType == TriggerTypes.OnTriggerEnter)
        {
            _onExecute?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!this.enabled) return;
        if (_triggerType == TriggerTypes.OnTriggerExit)
        {
            _onExecute?.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!this.enabled) return;
        if (_triggerType == TriggerTypes.OnCollisionEnter)
        {
            _onExecute?.Invoke();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!this.enabled) return;
        if (_triggerType == TriggerTypes.OnCollisionExit)
        {
            _onExecute?.Invoke();
        }
    }
}