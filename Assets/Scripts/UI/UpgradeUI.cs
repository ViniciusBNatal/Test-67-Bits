using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _moneyText;

    private void Start()
    {
        UpgradeManager.Instance.OnLevelUpdate += HandleLevelUpdate;
        UpgradeManager.Instance.OnMoneyUpdate += HandleMoneyUpdate;
    }

    private void HandleMoneyUpdate(float currentMoney)
    {
        _moneyText.text = currentMoney.ToString("0.");
    }

    private void HandleLevelUpdate(ushort currentLevel)
    {
        _levelText.text = currentLevel.ToString();
    }
}
