using UnityEngine;
using TMPro;

public class MoneyGenerator : MonoBehaviour
{

    public float baserate = 1.0f;
    // Update is called once per frame
    public float getCurrentRate()
    {
        float currentRate = baserate;
        currentRate += UpgradesManager.M_upgrades[0].level * 1f; // Example: Each level of the first mining upgrade adds 1 to the rate
        currentRate *= 1 + (UpgradesManager.M_upgrades[3].level * 0.04f * ResourceManager.Instance.totalApples * 0.01f); 
        return currentRate;
    }
    void Update()
    {
        float moneyGained = getCurrentRate() * Time.deltaTime;
        ResourceManager.Instance.totalMoney += moneyGained;
    }
}
