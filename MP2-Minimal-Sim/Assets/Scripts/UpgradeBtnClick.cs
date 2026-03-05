using UnityEngine;

public class UpgradeBtnClick : MonoBehaviour
{
    public void OnClick()
    {
        string upgradeName = gameObject.name.Replace("Btn", ""); // Assuming the button's name is like "Upgrade1Btn"
        foreach (var upgrade in UpgradesManager.M_upgrades)
        {
            if (upgrade.name == upgradeName)
            {
                if (ResourceManager.Instance.totalMoney >= upgrade.baseCost * Mathf.Pow(upgrade.costMultiplier, upgrade.level))
                {
                    ResourceManager.Instance.totalMoney -= upgrade.baseCost * Mathf.Pow(upgrade.costMultiplier, upgrade.level);
                    upgrade.level++;
                }
                break;
            }
        }
    }
}
