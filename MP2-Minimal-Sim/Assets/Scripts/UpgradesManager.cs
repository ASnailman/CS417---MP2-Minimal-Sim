using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{
    public static UpgradesManager Instance;

    public GameObject[] MiningUpgrades;
    public GameObject[] FarmingUpgrades;

    public Sprite AffordableUpgradeSprite;
    public Sprite UnaffordableUpgradeSprite;

    private double[] miningBaseCosts = { 20, 200, 1000, 40000 };
    private double[] miningCostMultipliers = { 1.05, 1.1, 1.15, 1.2 };
    private double[] farmingBaseCosts = { 100, 5, 15, 50, 300 }; 
    private double[] farmingCostMultipliers = { 5, 1.1, 1.15, 1.2, 1.25 };

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
        public TextMeshProUGUI buttonText; 
        public int MaxLvl = 999;
        public Button button;
    }
    
    public static List<Upgrade> M_upgrades = new List<Upgrade>(); 
    public static List<Upgrade> F_upgrades = new List<Upgrade>();

    public int TotalUpgradeLevel = 0;

    private void Awake() //
    {
        Instance = this; //

        // Clear static lists to prevent duplicates on scene reload
        M_upgrades.Clear(); 
        F_upgrades.Clear();

        for (int i = 0; i < MiningUpgrades.Length; i++)
        {
            Upgrade upgrade = new Upgrade();
            upgrade.name = MiningUpgrades[i].name;
            upgrade.nameText = MiningUpgrades[i].transform.Find(upgrade.name + "Lvl").GetComponent<TextMeshProUGUI>();
            upgrade.descriptionText = MiningUpgrades[i].transform.Find(upgrade.name + "Txt").GetComponent<TextMeshProUGUI>();
            upgrade.buttonText = MiningUpgrades[i].transform.Find(upgrade.name + "Btn").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            upgrade.baseCost = miningBaseCosts[i];
            upgrade.costMultiplier = miningCostMultipliers[i];
            upgrade.description = upgrade.descriptionText.text;
            upgrade.button = MiningUpgrades[i].transform.Find(upgrade.name + "Btn").GetComponent<Button>();
            M_upgrades.Add(upgrade);
        }

        for (int i = 0; i < FarmingUpgrades.Length; i++)
        {
            Upgrade upgrade = new Upgrade();
            upgrade.name = FarmingUpgrades[i].name;
            upgrade.nameText = FarmingUpgrades[i].transform.Find(upgrade.name + "Lvl").GetComponent<TextMeshProUGUI>();
            upgrade.descriptionText = FarmingUpgrades[i].transform.Find(upgrade.name + "Txt").GetComponent<TextMeshProUGUI>();
            upgrade.buttonText = FarmingUpgrades[i].transform.Find(upgrade.name + "Btn").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            upgrade.baseCost = farmingBaseCosts[i];
            upgrade.costMultiplier = farmingCostMultipliers[i];
            upgrade.description = upgrade.descriptionText.text;
            upgrade.button = FarmingUpgrades[i].transform.Find(upgrade.name + "Btn").GetComponent<Button>();  
            F_upgrades.Add(upgrade);
        }

        if (F_upgrades.Count > 1)
        {
            F_upgrades[0].MaxLvl = 5;
            F_upgrades[1].RequiresPreviousLevel = 1;
        }
    }

    void Update() //
    {
        if (Instance == null || ResourceManager.Instance == null || M_upgrades.Count == 0 || F_upgrades.Count == 0)
        {
            return;
        }

        // Existing Update logic remains the same
        for (int i = 0; i < M_upgrades.Count; i++)
        {
            Upgrade upgrade = M_upgrades[i];
            if (M_upgrades[i].level >= M_upgrades[i].MaxLvl)
            {
                upgrade.button.interactable = false;
                upgrade.buttonText.text = "MAX";
                upgrade.button.GetComponent<Image>().sprite = UnaffordableUpgradeSprite;
                continue;
            }
            if (i == 0 || M_upgrades[i-1].level >= upgrade.RequiresPreviousLevel)
            {
                double cost = upgrade.baseCost * System.Math.Pow(upgrade.costMultiplier, upgrade.level);
                upgrade.button.GetComponent<Image>().sprite = AffordableUpgradeSprite;
                upgrade.nameText.text = $"{upgrade.name} Lv.{upgrade.level}";
                
                if (i == 0) upgrade.descriptionText.text = $"{upgrade.description}\n+ {upgrade.level} -> {upgrade.level+1}";
                else if (i == 1) upgrade.descriptionText.text = $"{upgrade.description}\n+ {upgrade.level * 10} -> {(upgrade.level + 1) * 10}";
                else if (i == 2) upgrade.descriptionText.text = $"{upgrade.description}\n+ {upgrade.level * 2}% -> {upgrade.level * 2 + 2}%";
                else if (i == 3) upgrade.descriptionText.text = $"{upgrade.description}\n+ {upgrade.level * 4}% -> {(upgrade.level + 1) * 4}%";

                upgrade.buttonText.text = $"${System.Math.Floor(cost)}";
                if (ResourceManager.Instance.totalMoney >= cost)
                {
                    upgrade.button.interactable = true;
                    upgrade.buttonText.color = Color.black;
                }
                else
                {
                    upgrade.buttonText.color = Color.red;
                    upgrade.button.interactable = false;
                }
            }
            else
            {
                MiningUpgrades[i].SetActive(M_upgrades[i-1].level != 0);
                upgrade.button.GetComponent<Image>().sprite = UnaffordableUpgradeSprite;
                upgrade.button.interactable = false;
            }
        }

        for (int i=0; i < F_upgrades.Count; i++)
        {
            Upgrade upgrade = F_upgrades[i];
            if (F_upgrades[i].level >= F_upgrades[i].MaxLvl)
            {
                upgrade.button.interactable = false;
                upgrade.buttonText.text = "MAX";
                upgrade.button.GetComponent<Image>().sprite = UnaffordableUpgradeSprite;
                continue;
            }
            if (i == 0 || F_upgrades[i-1].level >= upgrade.RequiresPreviousLevel)
            {
                FarmingUpgrades[i].SetActive(true);
                double cost = upgrade.baseCost * System.Math.Pow(upgrade.costMultiplier, upgrade.level);
                upgrade.button.GetComponent<Image>().sprite = AffordableUpgradeSprite;
                upgrade.nameText.text = $"{upgrade.name} Lv.{upgrade.level}";
                
                if (i == 1) upgrade.descriptionText.text = $"{upgrade.description}\n+ {10 * upgrade.level}% -> {10 * (upgrade.level + 1)}%";
                else if (i == 2) upgrade.descriptionText.text = $"{upgrade.description}\n+ {upgrade.level * 0.05f * 100}% -> {(upgrade.level + 1) * 0.05f * 100}%";
                else if (i == 3) upgrade.descriptionText.text = $"{upgrade.description} + {upgrade.level} -> {upgrade.level + 1} and + {upgrade.level * 0.02f * 100}% -> {(upgrade.level + 1) * 0.02f * 100}%";
                else if (i == 4) upgrade.descriptionText.text = $"{upgrade.description}\n+ {upgrade.level * 0.05f * 100}% -> {(upgrade.level + 1) * 0.05f * 100}%";
                else upgrade.descriptionText.text = $"{upgrade.description}\n+ {upgrade.level} -> {upgrade.level+1}";

                if (i == 0)
                {
                    upgrade.buttonText.text = $"${System.Math.Floor(cost)}";
                    bool canAfford = ResourceManager.Instance.totalMoney >= cost;
                    upgrade.button.interactable = canAfford;
                    upgrade.buttonText.color = canAfford ? Color.black : Color.red;
                }
                else
                {
                    upgrade.buttonText.text = $"{System.Math.Floor(cost)} Apples";
                    bool canAfford = ResourceManager.Instance.totalApples >= System.Math.Floor(cost);
                    upgrade.button.interactable = canAfford;
                    upgrade.buttonText.color = canAfford ? Color.black : Color.red;
                }
            }
            else
            {
                FarmingUpgrades[i].SetActive(F_upgrades[i-1].level != 0);
                upgrade.button.GetComponent<Image>().sprite = UnaffordableUpgradeSprite;
                upgrade.button.interactable = false;
            }
        }
    }
}