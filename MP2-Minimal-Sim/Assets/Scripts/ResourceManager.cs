using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public double totalMoney = 0;
    public int totalApples = 0;

    public float water = 0f;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI appleText;
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI MiningBtnText;
    public EaseIn moneyTextEase;
    public EaseIn mineTextEase;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (moneyText != null) 
            moneyText.text = $"${System.Math.Floor(totalMoney)}";
            
        if (appleText != null) 
            appleText.text = $"Apples: {totalApples}";
        
        if (MiningBtnText != null)
        {
            double mining_rate = MiningGainMoney.getCurrentRate();
            MiningBtnText.text = $"+${mining_rate}";
        }
        if (waterText != null)
        {
            waterText.text = $"Water: {Mathf.FloorToInt(water)} / {Mathf.FloorToInt((UpgradesManager.F_upgrades[1].level*0.1f + 1) * 100)}";
        }
    }
}