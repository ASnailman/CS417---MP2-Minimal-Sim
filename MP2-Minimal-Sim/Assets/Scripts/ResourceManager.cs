using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public float totalMoney = 0f;
    public float totalApples = 0f;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI appleText;

    public TextMeshProUGUI MiningBtnText;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (moneyText != null) 
            moneyText.text = $"${Mathf.FloorToInt(totalMoney)}";
            
        if (appleText != null) 
            appleText.text = $"Apples: {Mathf.FloorToInt(totalApples)}";
        
        if (MiningBtnText != null)
        {
            float mining_rate = MiningGainMoney.getCurrentRate();
            MiningBtnText.text = $"+${mining_rate}";
        }
    }
}