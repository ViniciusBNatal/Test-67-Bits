using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopType _shopMode;

    private enum ShopType
    {
        Buy,
        Sell
    }

    public void MakeAction()
    {
        switch (_shopMode)
        {
            case ShopType.Buy:
                UpgradeManager.Instance.UpdateLevel();
                break;
            case ShopType.Sell:
                UpgradeManager.Instance.UpdateMoney();
                break;
        }
    }
}
