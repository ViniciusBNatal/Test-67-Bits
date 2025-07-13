using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementControls : MonoBehaviour
{
    [SerializeField] private Transform _cameraRotation;
    [SerializeField] private Transform _characterModel;
    [SerializeField, Min(0f)] private float _speed;
    [SerializeField] private float _turnSpeed;

    private CharacterController _characterControler;
    private Vector2 _movementInput;

    public Action<bool> OnInputUpdate;

    private void Start()
    {
        _characterControler = GetComponent<CharacterController>();
        InputManager.Instance.Inputs.Player.Move.performed += HandleMovePerformed;
    }

    private void FixedUpdate()
    {
        UpdateMovement();
        UpdateVisualRotation();
    }

    private void HandleMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _movementInput = obj.ReadValue<Vector2>().normalized;
        OnInputUpdate?.Invoke(_movementInput != Vector2.zero);
    }

    private void UpdateVisualRotation()
    {
        _characterModel.rotation = Quaternion.Lerp(_characterModel.rotation, Quaternion.Euler(0, _cameraRotation.rotation.eulerAngles.y, 0), _turnSpeed * Time.fixedDeltaTime);
    }

    private void UpdateMovement()
    {
        Vector3 finalSpeed = _movementInput.x * _cameraRotation.right + _movementInput.y * _cameraRotation.forward;
        finalSpeed = _speed * Time.fixedDeltaTime * new Vector3(finalSpeed.x, 0, finalSpeed.z);
        _characterControler.Move(finalSpeed);
    }    
}
