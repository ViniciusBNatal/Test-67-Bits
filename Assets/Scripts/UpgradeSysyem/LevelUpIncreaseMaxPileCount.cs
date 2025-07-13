using UnityEngine;
using System;

public class LevelUpIncreaseMaxPileCount : LevelUpChange
{
    [SerializeField] private PileCountData[] _maxPileIncreaseByLevelList;

    private int _previousListIndex;

    [Serializable]
    private struct PileCountData
    {
        public ushort LevelRequired;
        public byte MaxCapIncrease;
    }

    protected override void HandleLevelUpdate(ushort currentlevel)
    {
        for(int i = _previousListIndex; i < _maxPileIncreaseByLevelList.Length; i++)
        {
            if(_maxPileIncreaseByLevelList[i].LevelRequired <= currentlevel)
            {
                PileManager.Instance.IncreasePileMaxCap(_maxPileIncreaseByLevelList[i].MaxCapIncrease);
            }
            else
            {
                _previousListIndex = i;
                break;
            }
        }
    }
}
