using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private float _sensitivity;
    [SerializeField] private Transform _currentTarget;
    [SerializeField, Range(0f, 1f)] private float _dragAreaPrecentX;
    [SerializeField, Range(0f, 1f)] private float _dragAreaPrecentY;
    [SerializeField] private RectTransform _area;

    private Vector2 _touchPosition;

//#if UNITY_EDITOR
//    private void Awake()
//    {
//        RecalcDebugArea();
//    }
//#endif

    private void Start()
    {
        InputManager.Instance.Inputs.Player.Look.performed += HandleLookPerformed;
        InputManager.Instance.Inputs.Player.Touch.performed += HandleTouchPerformed;
    }

    private void HandleLookPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Vector2 input = obj.ReadValue<Vector2>();
        if(input != Vector2.zero/* && CheckTouchArea()*/)
        {
            _currentTarget.eulerAngles += _sensitivity * new Vector3(-input.y, input.x, 0) / Screen.currentResolution.width;
        }
    }

    private void HandleTouchPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _touchPosition = obj.ReadValue<Vector2>();
    }
//    void Update()
//    {
//#if UNITY_EDITOR
//        if (_useKeyboard)
//        {
//            _currentTarget.eulerAngles += _sensitivity * Time.deltaTime * new Vector3(0, Input.GetAxis("Horizontal"), 0) / Screen.currentResolution.width;
//        }
//#endif
//        if (Input.touchCount > 0)
//        {
//            _input = Input.GetTouch(0);

    //            if (_input.phase == TouchPhase.Moved /*&& _input.deltaPosition.magnitude > _dragTresHold*/ && CheckTouchArea())
    //            {
    //                _currentTarget.eulerAngles += _sensitivity * Time.deltaTime * new Vector3(0, _input.deltaPosition.x, 0) / Screen.currentResolution.width;
    //            }
    //        }
    //    }



    private bool CheckTouchArea()
    {
        //DebugText.WriteText($"current {_input.position}. Target: {Screen.height}");
        //return _area.rect.Contains(_touchPosition);
        //Vector2 initialPos = new Vector2(Screen.width, 0);
        return _touchPosition.y < Screen.height * _dragAreaPrecentY &&
            _touchPosition.x < Screen.width * ((1 - _dragAreaPrecentX) / 2 + _dragAreaPrecentX) &&
            _touchPosition.x > Screen.width * (1 - _dragAreaPrecentX) / 2;
    }

#if UNITY_EDITOR
    [ContextMenu("RecalculateDebugArea")]
    private void RecalcDebugArea()
    {
        if (_area)
        {
            _area.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.currentResolution.width * _dragAreaPrecentX);
            _area.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.currentResolution.height * _dragAreaPrecentY);
        }
    }
#endif
}
