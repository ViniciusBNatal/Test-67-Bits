using UnityEngine;
using System;

public class LevelUpChangePlayerColor : LevelUpChange
{
    [SerializeField] private ColorData[] _colorByLevelList;    

    [Serializable]
    private struct ColorData
    {
        public ushort LevelRequired;
        public Color Color;
    }

    protected override void HandleLevelUpdate(ushort currentlevel)
    {
        for(int i = 0; i < _colorByLevelList.Length; i++)
        {
            if(_colorByLevelList[i].LevelRequired == currentlevel)
            {
                PlayerModel.Instance.UpdateColor(_colorByLevelList[i].Color);
                break;
            }
        }
    }
}
