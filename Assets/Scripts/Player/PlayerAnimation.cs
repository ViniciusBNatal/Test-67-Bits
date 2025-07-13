using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Transform _punchRotationBone;

    private Animator _animator;
    private static readonly int _isMovingBool = Animator.StringToHash("IsMoving");
    private static readonly int _punchTrigger = Animator.StringToHash("Punch");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        GetComponent<MovementControls>().OnInputUpdate += HandleOnInputUpdate;
        GetComponentInChildren<PunchBehaviour>().OnPunch += HandleOnPunch;
    }

    private void HandleOnInputUpdate(bool isMoving)
    {
        _animator.SetBool(_isMovingBool, isMoving);
    }

    private void HandleOnPunch(Vector3 targetPosition)
    {
        _punchRotationBone.rotation = new Quaternion(_punchRotationBone.rotation.x, Quaternion.LookRotation(targetPosition - transform.position).y, _punchRotationBone.rotation.z, _punchRotationBone.rotation.w);
        _animator.SetTrigger(_punchTrigger);
    }
}
