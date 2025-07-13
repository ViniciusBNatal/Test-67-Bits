using UnityEngine;
using System;

public class UpgradeManager : MonoSingleton<UpgradeManager>
{
    [SerializeField] private ObjectValueData[] _objectPricesList;
    [SerializeField] private float[] _moneyRequiredToLevelUp;

    private ushort _currentLevel;
    private float _currentMoney;

    public Action<float> OnMoneyUpdate;
    public Action<ushort> OnLevelUpdate;

    [Serializable]
    private struct ObjectValueData
    {
        [PoolType] public int PoolType;
        [Min(0f)] public float Value;
    }

    private void Start()
    {
        PileManager.Instance.OnRemoveFromPile += AddMoney;
        PileManager.Instance.OnPileClear += () => OnMoneyUpdate?.Invoke(_currentMoney);
    }

    public void UpdateMoney()
    {
        PileManager.Instance.ClearPile();
    }

    private void AddMoney(int objectToSellId)
    {
        _currentMoney += GetMoneyValueById();        

        float GetMoneyValueById()
        {
            for (int i = 0; i < _objectPricesList.Length; i++)
            {
                if (_objectPricesList[i].PoolType == objectToSellId)
                {
                    return _objectPricesList[i].Value;
                }
            }
            return 0;
        }
    }

    public void UpdateLevel()
    {
        if (_currentLevel >= _moneyRequiredToLevelUp.Length) return;
        for(int i = _currentLevel; i < _moneyRequiredToLevelUp.Length; i++)
        {
            if (_currentMoney >= _moneyRequiredToLevelUp[i])
            {
                _currentMoney -= _moneyRequiredToLevelUp[i];
                _currentLevel++;
            }
            else break;
        }
        OnLevelUpdate?.Invoke(_currentLevel);
        OnMoneyUpdate?.Invoke(_currentMoney);
    }
}
