using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
public class UpgradesManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static UpgradesManager Instance;

    public GameObject[] MiningUpgrades;

    public GameObject[] FarmingUpgrades;

    public Sprite AffordableUpgradeSprite;
    public Sprite UnaffordableUpgradeSprite;

    private double[] miningBaseCosts = {20, 200, 1000, 7500};
    private double[] miningCostMultipliers = {1.05, 1.1, 1.15, 1.2};
    private double[] farmingBaseCosts = {16000, 5, 100, 2000, 50000}; //Upgrades 2-5 are in Apples, not Money
    private double[] farmingCostMultipliers = {9, 1.1, 1.15, 1.2, 1.25};
    public class Upgrade
    {
        public string name;
        public string description;
        public double baseCost;
        public double costMultiplier;
        public int level = 0;
        public int RequiresPreviousLevel = 10;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI buttonText; // Reference to the button's TextMeshProUGUI component

        public int MaxLvl = 999;
        public Button button;
    }
    
    public static List<Upgrade> M_upgrades = new List<Upgrade>(); 
    public static List<Upgrade> F_upgrades = new List<Upgrade>();

    public int TotalUpgradeLevel = 0;
    void Start()
    {
        Instance = this;
        for (int i = 0; i < MiningUpgrades.Length; i++)
        {
            Upgrade upgrade = new Upgrade();
            upgrade.name = MiningUpgrades[i].name;
            upgrade.nameText = MiningUpgrades[i].transform.Find(upgrade.name+"Lvl").GetComponent<TextMeshProUGUI>();
            upgrade.descriptionText = MiningUpgrades[i].transform.Find(upgrade.name+"Txt").GetComponent<TextMeshProUGUI>();
            upgrade.buttonText = MiningUpgrades[i].transform.Find(upgrade.name+"Btn").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            upgrade.baseCost = miningBaseCosts[i];
            upgrade.costMultiplier = miningCostMultipliers[i];
            upgrade.description = upgrade.descriptionText.text;
            upgrade.button = MiningUpgrades[i].transform.Find(upgrade.name+"Btn").GetComponent<Button>();
            M_upgrades.Add(upgrade);
        }
        for (int i = 0; i < FarmingUpgrades.Length; i++)
        {
            Upgrade upgrade = new Upgrade();
            upgrade.name = FarmingUpgrades[i].name;
            upgrade.nameText = FarmingUpgrades[i].transform.Find(upgrade.name+"Lvl").GetComponent<TextMeshProUGUI>();
            upgrade.descriptionText = FarmingUpgrades[i].transform.Find(upgrade.name+"Txt").GetComponent<TextMeshProUGUI>();
            upgrade.buttonText = FarmingUpgrades[i].transform.Find(upgrade.name+"Btn").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            upgrade.baseCost = farmingBaseCosts[i];
            upgrade.costMultiplier = farmingCostMultipliers[i];
            upgrade.description = upgrade.descriptionText.text;
            upgrade.button = FarmingUpgrades[i].transform.Find(upgrade.name+"Btn").GetComponent<Button>();  
            F_upgrades.Add(upgrade);
        }
        F_upgrades[0].MaxLvl = 5;
        F_upgrades[1].RequiresPreviousLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < M_upgrades.Count; i++)
        {
            Upgrade upgrade = M_upgrades[i];
            if (M_upgrades[i].level >= M_upgrades[i].MaxLvl)
            {
                upgrade.button.interactable = false; // Disable the button if max level is reached
                upgrade.buttonText.text = "MAX";
                upgrade.button.GetComponent<Image>().sprite = UnaffordableUpgradeSprite; // Set to unaffordable sprite
                continue; // Skip the rest of the loop for this upgrade
            }
            if (i == 0 || M_upgrades[i-1].level >= upgrade.RequiresPreviousLevel) // Check if the previous upgrade is at the required level
            {
                double cost = upgrade.baseCost * System.Math.Pow(upgrade.costMultiplier, upgrade.level);
                upgrade.button.GetComponent<Image>().sprite = AffordableUpgradeSprite;
                upgrade.nameText.text = $"{upgrade.name} Lv.{upgrade.level}";
                if (i == 0)
                {
                    upgrade.descriptionText.text = $"{upgrade.description}\n+ {upgrade.level} -> {upgrade.level+1}";
                }
                else if (i == 1)
                {
                    upgrade.descriptionText.text = $"{upgrade.description}\n+ {upgrade.level * 10} -> {(upgrade.level + 1) * 10}";
                }
                else if (i == 2)
                {
                    upgrade.descriptionText.text = $"{upgrade.description}\n+ {upgrade.level * 2}% -> {upgrade.level * 2 + 2}%";
                }
                else if (i == 3)
                {
                    upgrade.descriptionText.text = $"{upgrade.description}\n+ {upgrade.level * 4}% -> {(upgrade.level + 1) * 4}%";
                }
                upgrade.buttonText.text = $"${System.Math.Floor(cost)}";
                if (ResourceManager.Instance.totalMoney >= cost)
                {
                    upgrade.button.interactable = true; // Enable the button if affordable
                    upgrade.buttonText.color = Color.black; // Affordable
                }
                else
                {

                    upgrade.buttonText.color = Color.red; // Unaffordable
                    upgrade.button.interactable = false; // Disable the button if unaffordable
                }
            }
            else
            {
                if (M_upgrades[i-1].level == 0)
                {
                    MiningUpgrades[i].SetActive(false); // Hide the upgrade if the previous one hasn't been purchased at all
                    upgrade.button.GetComponent<Image>().sprite = UnaffordableUpgradeSprite; // Set to unaffordable sprite
                    upgrade.button.interactable = false; // Disable the button
                }
                else
                {
                    MiningUpgrades[i].SetActive(true); // Show the upgrade but mark it as unaffordable if the previous one hasn't reached the required level
                    upgrade.button.GetComponent<Image>().sprite = UnaffordableUpgradeSprite; // Set to unaffordable sprite
                    upgrade.button.interactable = false; // Disable the button
                }
            }
        }
        for (int i=0; i < F_upgrades.Count; i++)
            {
                Upgrade upgrade = F_upgrades[i];
                if (i == 0 || F_upgrades[i-1].level >= upgrade.RequiresPreviousLevel) // Check if the previous upgrade is at the required level
                {
                    FarmingUpgrades[i].SetActive(true); // Ensure the upgrade is visible if it's unlocked
                    double cost = upgrade.baseCost * System.Math.Pow(upgrade.costMultiplier, upgrade.level);
                    upgrade.button.GetComponent<Image>().sprite = AffordableUpgradeSprite;
                    upgrade.nameText.text = $"{upgrade.name} Lv.{upgrade.level}";
                    upgrade.descriptionText.text = $"{upgrade.description}\n+ {upgrade.level} -> {upgrade.level+1}";
                    if (i == 1)
                    {
                        upgrade.descriptionText.text = $"{upgrade.description}\n+ {10 * upgrade.level}% -> {10 * (upgrade.level + 1)}%";
                    }
                    else if (i ==2){
                        upgrade.descriptionText.text = $"{upgrade.description}\n+ {upgrade.level * 0.05f * 100}% -> {(upgrade.level + 1) * 0.05f * 100}%";
                    }
                     else if (i ==3){
                        upgrade.descriptionText.text = $"{upgrade.description} + {upgrade.level} -> {upgrade.level + 1} and + {upgrade.level * 0.02f * 100}% -> {(upgrade.level + 1) * 0.02f * 100}%";
                    }
                     else if (i ==4){
                        upgrade.descriptionText.text = $"{upgrade.description}\n+ {upgrade.level * 0.05f * 100}% -> {(upgrade.level + 1) * 0.05f * 100}%";
                    }
                    if (i == 0){
                        upgrade.buttonText.text = $"${System.Math.Floor(cost)}";
                        if (ResourceManager.Instance.totalMoney >= cost)
                        {
                            upgrade.button.interactable = true; // Enable the button if affordable
                            upgrade.buttonText.color = Color.black; // Affordable
                        }
                        else
                        {
                            upgrade.buttonText.color = Color.red; // Unaffordable
                            upgrade.button.interactable = false; // Disable the button if unaffordable
                        }
                    }
                    else{
                        upgrade.buttonText.text = $"{System.Math.Floor(cost)} Apples";
                        if (ResourceManager.Instance.totalApples >= System.Math.Floor(cost))
                        {
                            upgrade.button.interactable = true; // Enable the button if affordable
                            upgrade.buttonText.color = Color.black; // Affordable
                        }
                        else
                        {
                            upgrade.buttonText.color = Color.red; // Unaffordable
                            upgrade.button.interactable = false; // Disable the button if unaffordable
                        }
                    }
                }
                else
                {
                    if (F_upgrades[i-1].level == 0)
                    {
                        FarmingUpgrades[i].SetActive(false); // Hide the upgrade if the previous one hasn't been purchased at all
                        upgrade.button.GetComponent<Image>().sprite = UnaffordableUpgradeSprite; // Set to unaffordable sprite
                        upgrade.button.interactable = false; // Disable the button
                    }
                    else
                    {
                        FarmingUpgrades[i].SetActive(true); // Show the upgrade but mark it as unaffordable if the previous one hasn't reached the required level
                        upgrade.button.GetComponent<Image>().sprite = UnaffordableUpgradeSprite; // Set to unaffordable sprite
                        upgrade.button.interactable = false; // Disable the button
                    }
                }
            }
    }

}
