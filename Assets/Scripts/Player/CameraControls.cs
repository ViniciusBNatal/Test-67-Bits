using System;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private float _sensitivity;
    //[SerializeField, Min(0f)] private float _maxAxisXAngle;
    [SerializeField, Range(0f, 1f)] private float _increaseCameraDistancePerPileObject = .2f;
    [SerializeField, Range(0f, 1f)] private float _dragAreaBlockPrecentX;
    [SerializeField, Range(0f, 1f)] private float _dragAreaBlockPrecentY;
    [SerializeField] private Transform _currentTarget;
    [SerializeField] private RectTransform _area;

    private Vector2 _touchPosition;
    private Vector3 _cameraInitialPosition;
    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = _currentTarget.GetChild(0).transform;
        _cameraInitialPosition = _cameraTransform.localPosition;
        InputManager.Instance.Inputs.Player.Look.performed += HandleLookPerformed;
        InputManager.Instance.Inputs.Player.Touch.performed += HandleTouchPerformed;
        PileManager.Instance.OnAddToPile += HandlePileUpdate;
        PileManager.Instance.OnPileClear += HandlePileClear;
    }

    private void HandlePileClear()
    {
        _cameraTransform.localPosition = _cameraInitialPosition;
    }

    private void HandlePileUpdate(float objectSize, int pileSize)
    {
        _cameraTransform.localPosition -= new Vector3(0, 0, objectSize + pileSize * _increaseCameraDistancePerPileObject);
    }

    private void HandleLookPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Vector2 input = obj.ReadValue<Vector2>();
        if (input != Vector2.zero && CheckTouchArea())
        {
            Vector3 finalRotation = _currentTarget.eulerAngles + _sensitivity * new Vector3(-input.y, input.x, 0) / Screen.currentResolution.width;
            //Debug.Log($"input {input.y}, final rot {finalRotation.x}");
            finalRotation = new Vector3(finalRotation.x, finalRotation.y, 0);
            //if ((input.y < 0 && finalRotation.x > _maxAxisXAngle) || (input.y > 0 && finalRotation.x < 360 - _maxAxisXAngle))
            //{
            //    finalRotation.x = input.y < 0 ? _maxAxisXAngle + 180 : _maxAxisXAngle;
            //    Debug.Log($"FINAL X {finalRotation.x}");
            //}
            _currentTarget.eulerAngles = finalRotation;
        }
    }

    private void HandleTouchPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _touchPosition = obj.ReadValue<Vector2>();
    }

    private bool CheckTouchArea()
    {
        return _touchPosition.y > Screen.height * _dragAreaBlockPrecentY &&
            _touchPosition.x > Screen.width * _dragAreaBlockPrecentX;
    }

#if UNITY_EDITOR
    [ContextMenu("RecalculateDebugArea")]
    private void RecalcDebugArea()
    {
        if (_area)
        {
            _area.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.currentResolution.width * _dragAreaBlockPrecentX);
            _area.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.currentResolution.height * _dragAreaBlockPrecentY);
        }
    }
#endif
}
