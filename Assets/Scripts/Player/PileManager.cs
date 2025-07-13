using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MovementControls))]
public class PileManager : MonoSingleton<PileManager>
{
    [SerializeField] private byte _maxPileObjects;
    [SerializeField] private byte _initialMaxPileObjects;
    [SerializeField, Range(0f, 180f)] private float _maxObjectAngle = 45f;
    [SerializeField, Min(1f)] private float _rotationMultipier;
    [SerializeField] private Transform _pilePivot;
    [SerializeField] private AnimationCurve _inertiaAnimationCurve;
    [SerializeField] private PileObjectSizeData[] _pileObjectsSizeData;

    private List<PileObjectData> _pileObjects = new List<PileObjectData>();
    private byte _currentMaxPileObjects;
    private Vector3 _movementDirection;
    private Coroutine _inertiaCoroutine;

    public Action<byte> OnUpdatePileMaxCount;
    public Action<int> OnRemoveFromPile;
    public Action<float, int> OnAddToPile;
    public Action OnPileClear;

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
        _currentMaxPileObjects = _initialMaxPileObjects;
        InputManager.Instance.Inputs.Player.Move.performed += HandleOnMovement;
    }    

    private Vector3 GetObjectPositionInPile(int index)
    {
        if (index > 0)
        {
            int previousIndex = index - 1;
            Vector4 temp = _pileObjects[previousIndex].Transform.worldToLocalMatrix * Vector3.up;

            return _pileObjects[previousIndex].Transform.localPosition +
                (_pileObjects[previousIndex].ObjectData.ObjectSize + _pileObjects[index].ObjectData.ObjectSize) / 2 *
                new Vector3(-temp.x, temp.y, -temp.z);
        }
        else return Vector3.zero;
    }

    private void HandleOnMovement(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Vector2 input = obj.ReadValue<Vector2>();
        _movementDirection = new Vector3(-input.y, 0, input.x);
    }

    private IEnumerator InertiaCoroutine()
    {
        Quaternion finalRotation;
        Vector3 finalPosition;
        Vector3 direction;
        byte updateSequenceCount = 0;
        float max = _maxPileObjects;
        while (_pileObjects.Count > 0)
        {
            direction = _movementDirection.x != 0 && _movementDirection.z != 0 ? -_movementDirection : _movementDirection;
            finalRotation = Quaternion.Euler(_rotationMultipier * direction);
            for (int i = 0; i < _pileObjects.Count; i++)
            {
                finalRotation = Quaternion.Lerp(_pileObjects[i].Transform.localRotation, finalRotation,
                    _inertiaAnimationCurve.Evaluate((i + 1) / max));

                finalPosition = GetObjectPositionInPile(i);

                if (Quaternion.Angle(Quaternion.identity, finalRotation) <= _maxObjectAngle * (i + 1) / max)
                {
                    _pileObjects[i].Transform.SetLocalPositionAndRotation(finalPosition, finalRotation);
                }

                if (updateSequenceCount == i)
                {
                    updateSequenceCount++;
                    if (updateSequenceCount >= _pileObjects.Count) updateSequenceCount = 0;
                    break;
                }
            }
            yield return null;
        }
        _inertiaCoroutine = null;
    }

    public void AddToPile(PoolObjectData poolData)
    {
        if (_pileObjects.Count + 1 <= _currentMaxPileObjects)
        {
            PoolingObject obj = GenericPoolManager.Instance.GetPoolingObject(poolData);
            PileObjectSizeData objectSizeData = GetObjectSize();
            PileObjectData objectData = new PileObjectData(obj.transform, objectSizeData);
            _pileObjects.Add(objectData);
            obj.transform.SetParent(_pilePivot);
            obj.transform.SetLocalPositionAndRotation(GetObjectPositionInPile(_pileObjects.Count - 1), Quaternion.identity);
            if (_inertiaCoroutine == null)
            {
                _inertiaCoroutine = StartCoroutine(InertiaCoroutine());
            }
            OnAddToPile?.Invoke(_pileObjects[^1].ObjectData.ObjectSize, _pileObjects.Count);
        }

        PileObjectSizeData GetObjectSize()
        {
            for (int i = 0; i < _pileObjectsSizeData.Length; i++)
            {
                if (_pileObjectsSizeData[i].PoolType == poolData.PoolType)
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
        OnUpdatePileMaxCount?.Invoke(_currentMaxPileObjects);
    }

    public void ClearPile()
    {
        for (int i = 0; i < _pileObjects.Count; i++)
        {
            OnRemoveFromPile?.Invoke(_pileObjects[i].ObjectData.PoolType);
            _pileObjects[i].Transform.gameObject.SetActive(false);
        }
        _pileObjects.Clear();
        OnPileClear?.Invoke();
    }
}
