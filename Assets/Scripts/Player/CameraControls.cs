using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private float _sensitivity;
    //[SerializeField, Min(0f)] private float _maxAxisXAngle;
    [SerializeField, Range(0f, 1f)] private float _increaseCameraDistancePerPileObject = .2f;
    [SerializeField, Range(0f, 1f)] private float _dragAreaBlockPrecentX;
    [SerializeField, Range(0f, 1f)] private float _dragAreaBlockPrecentY;
    [SerializeField] private Transform _currentTarget;
    [SerializeField] private RectTransform _area;

    private Vector2 _previousTouchPos;
    private Vector3 _cameraInitialPosition;
    private Transform _cameraTransform;
    private int _touchIndex = -1;

    private void Awake()
    {
        EnhancedTouchSupport.Enable();        
    }    

    private void Start()
    {
        _cameraTransform = _currentTarget.GetChild(0).transform;
        _cameraInitialPosition = _cameraTransform.localPosition;
        PileManager.Instance.OnAddToPile += HandlePileUpdate;
        PileManager.Instance.OnPileClear += HandlePileClear;

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += HandleOnFingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += HandleOnFingerMove;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += HandleOnFingerUp;
    }    

    private void HandlePileClear()
    {
        _cameraTransform.localPosition = _cameraInitialPosition;
    }

    private void HandlePileUpdate(float objectSize, int pileSize)
    {
        if (_cameraTransform) _cameraTransform.localPosition -= new Vector3(0, 0, objectSize + pileSize * _increaseCameraDistancePerPileObject);
    }

    private void HandleOnFingerDown(Finger obj)
    {
        if (CheckTouchArea(obj.currentTouch.startScreenPosition) && _touchIndex == -1) _touchIndex = obj.index;
    }

    private void HandleOnFingerMove(Finger obj)
    {
        if(obj.index == _touchIndex)
        {
            if(_previousTouchPos != Vector2.zero)
            {
                Vector2 input = (obj.screenPosition - _previousTouchPos).normalized;
                Vector3 finalRotation = _currentTarget.eulerAngles + _sensitivity * new Vector3(-input.y, input.x, 0) / Screen.currentResolution.width;
                finalRotation = new Vector3(finalRotation.x, finalRotation.y, 0);
                _currentTarget.eulerAngles = finalRotation;
            }
            _previousTouchPos = obj.screenPosition;
        }
    }

    private void HandleOnFingerUp(Finger obj)
    {
        if (obj.index == _touchIndex)
        {
            _touchIndex = -1;
            _previousTouchPos = Vector2.zero;
        }
    }

    private bool CheckTouchArea(Vector3 touchPosition)
    {
        return touchPosition.y > Screen.height * _dragAreaBlockPrecentY &&
            touchPosition.x > Screen.width * _dragAreaBlockPrecentX;
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
