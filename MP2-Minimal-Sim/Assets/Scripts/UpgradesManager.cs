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

    public GameObject[] Trees;
    private float[] miningBaseCosts = {20f, 200f, 1000f, 7500f};
    private float[] miningCostMultipliers = {1.05f, 1.1f, 1.15f, 1.2f};
    private float[] farmingBaseCosts = {16000f, 128000f, 1024000f, 8192000f};
    private float[] farmingCostMultipliers = {4f, 1.5f, 1.25f, 1.125f};
    public class Upgrade
    {
        public string name;
        public string description;
        public float baseCost;
        public float costMultiplier;
        public int level = 0;
        public int RequiresPreviousLevel = 10;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI buttonText; // Reference to the button's TextMeshProUGUI component

        public Button button;
    }
    
    public static List<Upgrade> M_upgrades = new List<Upgrade>(); 
    public static List<Upgrade> F_upgrades = new List<Upgrade>();


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
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < M_upgrades.Count; i++)
        {
            Upgrade upgrade = M_upgrades[i];
            if (i == 0 || M_upgrades[i-1].level >= upgrade.RequiresPreviousLevel) // Check if the previous upgrade is at the required level
            {
                float cost = upgrade.baseCost * Mathf.Pow(upgrade.costMultiplier, upgrade.level);
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
                else if (i == 2 || i == 3)
                {
                    upgrade.descriptionText.text = $"{upgrade.description}\n+ {upgrade.level}% -> {upgrade.level+1}%";
                }
                upgrade.buttonText.text = $"${Mathf.FloorToInt(cost)}";
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
                    float cost = upgrade.baseCost * Mathf.Pow(upgrade.costMultiplier, upgrade.level);
                    upgrade.button.GetComponent<Image>().sprite = AffordableUpgradeSprite;
                    upgrade.nameText.text = $"{upgrade.name} Lv.{upgrade.level}";
                    upgrade.descriptionText.text = $"{upgrade.description}\n+ {upgrade.level} -> {upgrade.level+1}";
                    if (i == 2 || i == 3)
                    {
                        upgrade.descriptionText.text = $"{upgrade.description}\n+ {upgrade.level}% -> {upgrade.level+1}%";
                    }
                    upgrade.buttonText.text = $"${Mathf.FloorToInt(cost)}";
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
