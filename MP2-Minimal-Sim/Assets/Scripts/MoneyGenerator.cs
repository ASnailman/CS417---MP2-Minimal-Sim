using UnityEngine;
using TMPro;

public class MoneyGenerator : MonoBehaviour
{
    public double baserate = 1.0;

    public double getCurrentRate() //
    {
        // Safety check: Ensure UpgradesManager has populated the list
        if (UpgradesManager.M_upgrades == null || UpgradesManager.M_upgrades.Count < 4)
        {
            return baserate;
        }

        double currentRate = baserate;
        currentRate += UpgradesManager.M_upgrades[0].level * 1.0; 
        currentRate *= 1 + UpgradesManager.M_upgrades[2].level * 0.02;
        currentRate *= 1 + (UpgradesManager.M_upgrades[3].level * 0.04 * ResourceManager.Instance.totalApples * 0.01); 
        return currentRate;
    }

    void Update() //
    {
        if (ResourceManager.Instance == null) return;
        double moneyGained = getCurrentRate() * Time.deltaTime;
        ResourceManager.Instance.totalMoney += moneyGained;
    }
}