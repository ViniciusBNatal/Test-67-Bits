using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PileManager : MonoSingleton<PileManager>
{
    [SerializeField] private PileObjectSizeData[] _pileObjectsSizeData;
    [SerializeField] private Transform _pilePivot;
    [SerializeField] private byte _maxPileObjects;
    [SerializeField] private byte _initialMaxPileObjects;
    [SerializeField] private AnimationCurve _inertiaAnimationCurve;
    //[SerializeField, Min(0f)] private float _maxObjectAngle;
    [SerializeField, Min(0f)] private float _tickFrequency;

    private List<PileObjectData> _pileObjects = new List<PileObjectData>();
    private byte _currentMaxPileObjects;
    private Vector2 _movementInput;
    private Coroutine _inertiaCoroutine;

    [Serializable]
    public struct PileObjectSizeData
    {
        [PoolType] public int PoolType;
        [Min(0f)] public float ObjectSize;
    }

    private struct PileObjectData
    {
        public Transform Transform;
        public PileObjectSizeData ObjectData;

        public PileObjectData(Transform transform, PileObjectSizeData objectdata)
        {
            Transform = transform;
            ObjectData = objectdata;
        }
    }

    private void Start()
    {
        InputManager.Instance.Inputs.Player.Move.performed += HandleMovePerformed;
        _currentMaxPileObjects = _initialMaxPileObjects;
    }

    private IEnumerator InertiaCoroutine()
    {
        WaitForSeconds delay = new WaitForSeconds(_tickFrequency);
        //Quaternion finalRotation;
        while (true)
        {
            for (int i = 0; i < _pileObjects.Count; i++)
            {
                //finalRotation = Quaternion.Euler(-_movementInput.normalized * (i + 1));
                _pileObjects[i].Transform.rotation = Quaternion.Lerp(_pileObjects[i].Transform.rotation,
                    Quaternion.Euler(-_movementInput * (i + 1)),
                    _inertiaAnimationCurve.Evaluate((i + 1) / _maxPileObjects));
            }
            yield return delay;
        }
    }

    private void HandleMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _movementInput = obj.ReadValue<Vector2>().normalized;
    }

    public void AddToPile(PoolObjectData poolData)
    {
        if (_pileObjects.Count + 1 <= _currentMaxPileObjects)
        {
            PoolingObject obj = GenericPoolManager.Instance.GetPoolingObject(poolData);
            _pileObjects.Add(new PileObjectData(obj.transform, GetObjectSize()));
            obj.transform.SetParent(_pilePivot);
            obj.transform.SetLocalPositionAndRotation(GetObjectPositionInPile(), Quaternion.identity);
            if (_inertiaCoroutine == null)
            {
                _inertiaCoroutine = StartCoroutine(InertiaCoroutine());
            }
        }

        Vector3 GetObjectPositionInPile()
        {
            if (_pileObjects.Count > 1)
            {
                int lastIndex = _pileObjects.Count - 1;
                return (_pileObjects[lastIndex - 1].ObjectData.ObjectSize + _pileObjects[lastIndex].ObjectData.ObjectSize) / 2 * Vector3.up * _pileObjects.Count;
            }
            else return Vector3.zero;
        }

        PileObjectSizeData GetObjectSize()
        {
            for (int i = 0; i < _pileObjectsSizeData.Length; i++)
            {
                if (_pileObjectsSizeData[i].PoolType == _pileObjects[^1].ObjectData.PoolType)
                {
                    return _pileObjectsSizeData[i];
                }
            }
            Debug.LogWarning($"The Object {poolData.name} is not present in the Object Size list");
            return new PileObjectSizeData();
        }
    }

    public void IncreasePileMaxCap(byte valueIncrease)
    {
        _currentMaxPileObjects = (byte)Mathf.Clamp(_currentMaxPileObjects + valueIncrease, 0, _maxPileObjects);
    }

    public void SellPile()
    {

    }
}
