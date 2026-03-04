using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public float totalMoney = 0f;
    public float totalApples = 0f;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI appleText;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (moneyText != null) 
            moneyText.text = $"Total Money: ${Mathf.FloorToInt(totalMoney)}";
            
        if (appleText != null) 
            appleText.text = $"Total Apples: {Mathf.FloorToInt(totalApples)}";
    }
}