using UnityEngine;

public class UpgradeBtnClick : MonoBehaviour
{

    bool moneyTutorialShown = false;
    bool appleTutorialShown = false;


    public void OnClick()
    {
        string upgradeName = gameObject.name.Replace("Btn", ""); // Assuming the button's name is like "Upgrade1Btn"
        Debug.Log("Button clicked!");
        foreach (var upgrade in UpgradesManager.M_upgrades)
        {
            if (upgrade.name == upgradeName)
            {
                if (upgradeName == "GrowthSpeed" || upgradeName == "IncreasedYield" || upgradeName == "StrengthenedSynergy") // These upgrades are in Apples, not Money
                {
                    if (ResourceManager.Instance.totalApples >= System.Math.Floor(upgrade.baseCost * System.Math.Pow(upgrade.costMultiplier, upgrade.level)))
                    {
                        ResourceManager.Instance.totalApples -= (int)(upgrade.baseCost * System.Math.Pow(upgrade.costMultiplier, upgrade.level));
                        upgrade.level++;
                        UpgradesManager.Instance.TotalUpgradeLevel++;
                    }
                }
                else
                {
                    
                    if (ResourceManager.Instance.totalMoney >= upgrade.baseCost * System.Math.Pow(upgrade.costMultiplier, upgrade.level))
                        {
                            ResourceManager.Instance.totalMoney -= upgrade.baseCost * System.Math.Pow(upgrade.costMultiplier, upgrade.level);
                            upgrade.level++;
                            UpgradesManager.Instance.TotalUpgradeLevel++;

                        //Tutorial trigger for money upgrades
                        if (upgradeName == "InterestScheme" && upgrade.level == 1 && !moneyTutorialShown)
                            {
                                FindObjectOfType<TutorialManager>().ShowMoneyTutorial();
                                moneyTutorialShown = true;
                            }
                    }
                }
                break;
            }
        }
        foreach (var upgrade in UpgradesManager.F_upgrades)
        {
            if (upgrade.name == upgradeName)
            {
                if (upgradeName == "GrowthSpeed" || upgradeName == "IncreasedYield" || upgradeName == "StrengthenedSynergy" || upgradeName == "LargerBucket") // These upgrades are in Apples, not Money
                {
                    if (ResourceManager.Instance.totalApples >= System.Math.Floor(upgrade.baseCost * System.Math.Pow(upgrade.costMultiplier, upgrade.level)))
                    {
                        ResourceManager.Instance.totalApples -= (int)(upgrade.baseCost * System.Math.Pow(upgrade.costMultiplier, upgrade.level));
                        upgrade.level++;
                        UpgradesManager.Instance.TotalUpgradeLevel++;
                    }
                }
                else
                {
                    
                    if (ResourceManager.Instance.totalMoney >= upgrade.baseCost * System.Math.Pow(upgrade.costMultiplier, upgrade.level))
                        {
                            ResourceManager.Instance.totalMoney -= upgrade.baseCost * System.Math.Pow(upgrade.costMultiplier, upgrade.level);
                            upgrade.level++;
                            UpgradesManager.Instance.TotalUpgradeLevel++;

                        //Tutorial trigger for apple growth upgrades
                        if (upgradeName == "GardenPlots" && upgrade.level == 1 && !appleTutorialShown)
                            {
                                FindObjectOfType<TutorialManager>().ShowAppleTutorial();
                                appleTutorialShown = true;
                            }
                    }
                }
                break;
            }
        }
    }

}
