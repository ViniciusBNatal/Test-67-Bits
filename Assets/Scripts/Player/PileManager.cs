using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileManager : MonoSingleton<PileManager>
{
    [SerializeField, Min(0f)] private float _spaceBetweenObjects;
    [SerializeField] private Transform _pilePivot;
    [SerializeField] private byte _maxPileObjects;
    [SerializeField] private byte _initialMaxPileObjects;
    [SerializeField] private AnimationCurve _inertiaAnimationCurve;
    //[SerializeField, Min(0f)] private float _maxObjectAngle;
    [SerializeField, Min(0f)] private float _tickFrequency;

    private List<Transform> _pileObjects = new List<Transform>();
    private byte _currentMaxPileObjects;
    private Vector2 _movementInput;
    private Coroutine _inertiaCoroutine;

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
                _pileObjects[i].transform.rotation = Quaternion.Lerp(_pileObjects[i].transform.rotation,
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
            obj.transform.SetParent(_pilePivot);
            obj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            if (_inertiaCoroutine == null)
            {
                _inertiaCoroutine = StartCoroutine(InertiaCoroutine());
            }
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
