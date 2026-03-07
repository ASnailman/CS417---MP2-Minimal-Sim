using UnityEngine;

public class UpgradeBtnClick : MonoBehaviour
{
    bool moneyTutorialShown = false;
    bool appleTutorialShown = false;

    public void OnClick() //
    {
        // Safety check: Ensure UpgradesManager is initialized
        if (UpgradesManager.M_upgrades == null || UpgradesManager.F_upgrades == null)
        {
            Debug.LogWarning("Upgrades lists not yet initialized!");
            return;
        }

        string upgradeName = gameObject.name.Replace("Btn", ""); 
        Debug.Log("Button clicked: " + upgradeName);

        // Check Mining Upgrades
        foreach (var upgrade in UpgradesManager.M_upgrades)
        {
            if (upgrade.name == upgradeName)
            {
                ProcessUpgrade(upgrade, true); // Mining upgrades always use Money
                return; // Found and processed, exit function
            }
        }

        // Check Farming Upgrades
        foreach (var upgrade in UpgradesManager.F_upgrades)
        {
            if (upgrade.name == upgradeName)
            {
                // Logic to determine if this farming upgrade costs Apples or Money
                bool costsApples = (upgradeName == "GrowthSpeed" || 
                                    upgradeName == "IncreasedYield" || 
                                    upgradeName == "StrengthenedSynergy" || 
                                    upgradeName == "LargerBucket");

                ProcessUpgrade(upgrade, !costsApples);
                return;
            }
        }
    }

    private void ProcessUpgrade(UpgradesManager.Upgrade upgrade, bool usesMoney) //
    {
        double cost = upgrade.baseCost * System.Math.Pow(upgrade.costMultiplier, upgrade.level);

        if (usesMoney)
        {
            if (ResourceManager.Instance.totalMoney >= cost)
            {
                ResourceManager.Instance.totalMoney -= cost;
                FinalizeUpgrade(upgrade);

                // Tutorial trigger for money upgrades
                if (upgrade.name == "InterestScheme" && upgrade.level == 1 && !moneyTutorialShown)
                {
                    var tutorial = FindFirstObjectByType<TutorialManager>();
                    if (tutorial != null) tutorial.ShowMoneyTutorial();
                    moneyTutorialShown = true;
                }
            }
        }
        else
        {
            if (ResourceManager.Instance.totalApples >= (int)cost)
            {
                ResourceManager.Instance.totalApples -= (int)cost;
                FinalizeUpgrade(upgrade);

                // Tutorial trigger for apple growth upgrades
                if (upgrade.name == "GardenPlots" && upgrade.level == 1 && !appleTutorialShown)
                {
                    var tutorial = FindFirstObjectByType<TutorialManager>();
                    if (tutorial != null) tutorial.ShowAppleTutorial();
                    appleTutorialShown = true;
                }
            }
        }
    }

    private void FinalizeUpgrade(UpgradesManager.Upgrade upgrade) //
    {
        upgrade.level++;
        UpgradesManager.Instance.TotalUpgradeLevel++;
        Debug.Log($"{upgrade.name} upgraded to level {upgrade.level}");
    }
}