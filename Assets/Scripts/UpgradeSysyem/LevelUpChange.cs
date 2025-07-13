using UnityEngine;

public abstract class LevelUpChange : MonoBehaviour
{
    protected virtual void Start()
    {
        UpgradeManager.Instance.OnLevelUpdate += HandleLevelUpdate;
    }

    protected abstract void HandleLevelUpdate(ushort currentlevel);
}