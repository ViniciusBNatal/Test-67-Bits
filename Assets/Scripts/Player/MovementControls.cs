using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementControls : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _speed;
    [SerializeField] private Transform _cameraRotation;

    private CharacterController _characterControler;
    private Vector2 _movementInput;

    public Action<bool> OnInputUpdate;

    private void Start()
    {
        _characterControler = GetComponent<CharacterController>();
        InputManager.Instance.Inputs.Player.Move.performed += HandleMovePerformed;
    }

    private void HandleMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _movementInput = obj.ReadValue<Vector2>();
        OnInputUpdate?.Invoke(_movementInput != Vector2.zero);
    }

    private void FixedUpdate()
    {
        Vector3 finalDirection = _movementInput.x * _cameraRotation.right + _movementInput.y * _cameraRotation.forward;
        finalDirection = new Vector3(finalDirection.x, 0, finalDirection.z);
        _characterControler.Move(_speed * Time.fixedDeltaTime * finalDirection);
    }
}
