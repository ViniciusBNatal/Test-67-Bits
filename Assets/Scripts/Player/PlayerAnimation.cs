using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Transform _punchRotationBone;
    [SerializeField] private int _punchAnimationLayer = 1;

    private Animator _animator;
    private Coroutine _animationEndCoroutine;
    private Quaternion _initialRotation;
    private static readonly int _isMovingBool = Animator.StringToHash("IsMoving");
    private static readonly int _punchTrigger = Animator.StringToHash("Punch");
    private static readonly int _puchState = Animator.StringToHash("punch");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _initialRotation = _punchRotationBone.localRotation;
        GetComponent<MovementControls>().OnInputUpdate += HandleOnInputUpdate;
        GetComponentInChildren<PunchBehaviour>().OnPunch += HandleOnPunch;
    }

    private void HandleOnInputUpdate(bool isMoving)
    {
        _animator.SetBool(_isMovingBool, isMoving);
    }

    private void HandleOnPunch(Vector3 targetPosition)
    {
        _punchRotationBone.localRotation = new Quaternion(_punchRotationBone.localRotation.x, Quaternion.LookRotation(targetPosition - transform.position).y, _punchRotationBone.localRotation.z, _punchRotationBone.localRotation.w);
        _animator.SetTrigger(_punchTrigger);
        if(_animationEndCoroutine != null)
        {
            StopCoroutine(_animationEndCoroutine);
            _animationEndCoroutine = null;
        }
        _animationEndCoroutine = StartCoroutine(WaitAnimationEndCoroutine());
    }

    private IEnumerator WaitAnimationEndCoroutine()
    {
        if (_animator.GetCurrentAnimatorStateInfo(_punchAnimationLayer).shortNameHash != _puchState)
        {
            while (_animator.GetCurrentAnimatorStateInfo(_punchAnimationLayer).shortNameHash != _puchState)
            {
                yield return null;
            }
        }
        while (_animator.GetCurrentAnimatorStateInfo(_punchAnimationLayer).shortNameHash == _puchState)
        {
            yield return null;
        }
        _punchRotationBone.localRotation = _initialRotation;
        _animationEndCoroutine = null;
    }
}
