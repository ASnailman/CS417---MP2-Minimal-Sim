using UnityEngine;
using TMPro;

public class MoneyGenerator : MonoBehaviour
{

    public double baserate = 1.0;
    // Update is called once per frame
    public double getCurrentRate()
    {
        double currentRate = baserate;
        currentRate += UpgradesManager.M_upgrades[0].level * 1.0; 
        currentRate *= 1 + UpgradesManager.M_upgrades[2].level * 0.02;
        currentRate *= 1 + (UpgradesManager.M_upgrades[3].level * 0.04 * ResourceManager.Instance.totalApples * 0.01); 
        return currentRate;
    }
    void Update()
    {
        double moneyGained = getCurrentRate() * Time.deltaTime;
        ResourceManager.Instance.totalMoney += moneyGained;
    }
}
