using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MiningGainMoney : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static MiningGainMoney Instance;
    public double baseRate = 1.0;

    public TextMeshProUGUI UICanvasMessageText;
    public Button mineButton;
    private void Awake()
    {
        Instance = this;
    }

    public static double getCurrentRate()
    {
        double currentRate = Instance.baseRate;
        currentRate += UpgradesManager.M_upgrades[1].level * 10;
        currentRate *= 1 + UpgradesManager.M_upgrades[2].level * 0.02;
        currentRate *= 1 + (UpgradesManager.M_upgrades[3].level * 0.04 * ResourceManager.Instance.totalApples * 0.01);
        return currentRate;
    }

    public void Mine()
    {
        double moneyGained = getCurrentRate();
        float DoubleDrop = Random.Range(0f, 1f);
        bool flgDoubleDrop = false;
        if (DoubleDrop <= UpgradesManager.M_upgrades[2].level * 0.02f)
        {
            moneyGained *= 2; 
            flgDoubleDrop = true;
        }
        ResourceManager.Instance.totalMoney += moneyGained;
        mineButton.interactable = false; // Disable the button immediately after clicking
        Invoke(nameof(EnableMineButton), 0.5f); // Re-enable the button after 0.5 second
        if (flgDoubleDrop)
        {
            UICanvasMessageText.text = "Double Drop!";
            UICanvasMessageText.color = Color.yellow; // Set the text color to yellow for double drop
            Invoke(nameof(ClearMessage), 1.0f); // Clear the message after 1 second
        }
    }
    private void EnableMineButton()
    {
        mineButton.interactable = true; // Re-enable the button
    }
    private void ClearMessage()
    {
        UICanvasMessageText.text = ""; // Clear the message
    }
    void Update()
    {
        mineButton.transform.Find("MiningAreaBtnTxt").GetComponent<TextMeshProUGUI>().text = $"+${System.Math.Floor(getCurrentRate())}";
    }
}
