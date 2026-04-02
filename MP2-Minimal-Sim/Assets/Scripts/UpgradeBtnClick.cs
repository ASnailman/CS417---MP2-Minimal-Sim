using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using TMPro;
using System.Collections;

public class UpgradeBtnClick : MonoBehaviour
{
    bool moneyTutorialShown = false;
    bool appleTutorialShown = false;
    private Coroutine costEaseCoroutine;

    [SerializeField] private float costEaseDuration = 0.2f;

    public AudioSource upgradeSound;
    public AudioSource GardenPlotSound;
    public HapticImpulsePlayer leftHaptic;
    public HapticImpulsePlayer rightHaptic;

    public ParticleSystem upgradeParticles;
    public void OnClick() //
    {
        // Safety check: Ensure UpgradesManager is initialized
        if (UpgradesManager.M_upgrades == null || UpgradesManager.F_upgrades == null)
        {
            Debug.LogWarning("Upgrades lists not yet initialized!");
            return;
        }

        string upgradeName = gameObject.name.Replace("Btn", ""); 
        Debug.Log("Button clicked: " + upgradeName);

        // Check Mining Upgrades
        foreach (var upgrade in UpgradesManager.M_upgrades)
        {
            if (upgrade.name == upgradeName)
            {
                ProcessUpgrade(upgrade, true); // Mining upgrades always use Money
                return; // Found and processed, exit function
            }
        }

        // Check Farming Upgrades
        foreach (var upgrade in UpgradesManager.F_upgrades)
        {
            if (upgrade.name == upgradeName)
            {
                // Logic to determine if this farming upgrade costs Apples or Money
                bool costsApples = (upgradeName == "GrowthSpeed" || 
                                    upgradeName == "IncreasedYield" || 
                                    upgradeName == "StrengthenedSynergy" || 
                                    upgradeName == "LargerBucket");

                ProcessUpgrade(upgrade, !costsApples);
                return;
            }
        }
    }

    private void ProcessUpgrade(UpgradesManager.Upgrade upgrade, bool usesMoney) //
    {
        double cost = upgrade.baseCost * System.Math.Pow(upgrade.costMultiplier, upgrade.level);

        if (usesMoney)
        {
            if (ResourceManager.Instance.totalMoney >= cost)
            {
                ResourceManager.Instance.totalMoney -= cost;
                FinalizeUpgrade(upgrade);

                // Tutorial trigger for money upgrades
                if (upgrade.name == "InterestScheme" && upgrade.level == 1 && !moneyTutorialShown)
                {
                    var tutorial = FindFirstObjectByType<TutorialManager>();
                    if (tutorial != null) tutorial.ShowMoneyTutorial();
                    moneyTutorialShown = true;
                }
                // Tutorial trigger for apple growth upgrades
                if (upgrade.name == "GardenPlots" && upgrade.level == 1 && !appleTutorialShown)
                {
                    var tutorial = FindFirstObjectByType<TutorialManager>();
                    if (tutorial != null) tutorial.ShowAppleTutorial();
                    appleTutorialShown = true;
                }
            }
        }
        else
        {
            if (ResourceManager.Instance.totalApples >= (int)cost)
            {
                ResourceManager.Instance.totalApples -= (int)cost;
                FinalizeUpgrade(upgrade);

            }
        }
    }

    private void FinalizeUpgrade(UpgradesManager.Upgrade upgrade) //
    {
        double previousCost = upgrade.baseCost * System.Math.Pow(upgrade.costMultiplier, upgrade.level);
        upgrade.level++;
        UpgradesManager.Instance.TotalUpgradeLevel++;
        Debug.Log($"{upgrade.name} upgraded to level {upgrade.level}");
        if (upgradeSound != null)
        {
            upgradeSound.volume = 1f;
            if (upgrade.name == "GardenPlots" && GardenPlotSound != null)
            {
                GardenPlotSound.transform.position = transform.position; // Move sound to button position
                GardenPlotSound.volume = 1f;
                GardenPlotSound.Play();
            }
            else
            {
                upgradeSound.Play();
            }
        }
        leftHaptic?.SendHapticImpulse(0.5f, 0.1f);
        rightHaptic?.SendHapticImpulse(0.5f, 0.1f);
        if (upgradeParticles != null)
        {
            upgradeParticles.transform.position = transform.position; // Move particle system to button position
            upgradeParticles.Emit(1);
        }
        double nextCost = upgrade.baseCost * System.Math.Pow(upgrade.costMultiplier, upgrade.level);
        EaseInCost(previousCost, nextCost, upgrade);
    }

    private void EaseInCost(double fromCost, double toCost, UpgradesManager.Upgrade upgrade)
    {
        if (upgrade == null || upgrade.buttonText == null)
        {
            return;
        }

        if (costEaseCoroutine != null)
        {
            StopCoroutine(costEaseCoroutine);
        }

        costEaseCoroutine = StartCoroutine(EaseInCostRoutine(fromCost, toCost, upgrade));
    }

    private IEnumerator EaseInCostRoutine(double fromCost, double toCost, UpgradesManager.Upgrade upgrade)
    {
        bool usesMoney = IsMoneyUpgrade(upgrade);

        if (costEaseDuration <= 0f)
        {
            upgrade.buttonText.text = FormatCostText(toCost, usesMoney);
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < costEaseDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / costEaseDuration);
            // Smoothstep gives a simple ease-in/ease-out interpolation.
            float easedT = t * t * (3f - 2f * t);
            double value = Mathf.Lerp((float)fromCost, (float)toCost, easedT);
            upgrade.buttonText.text = FormatCostText(value, usesMoney);
            yield return null;
        }

        upgrade.buttonText.text = FormatCostText(toCost, usesMoney);
        costEaseCoroutine = null;
    }

    private bool IsMoneyUpgrade(UpgradesManager.Upgrade upgrade)
    {
        // Mining upgrades and the first farming upgrade (GardenPlots) use money.
        if (UpgradesManager.M_upgrades.Contains(upgrade))
        {
            return true;
        }

        return upgrade.name == "GardenPlots";
    }

    private string FormatCostText(double cost, bool usesMoney)
    {
        double flooredCost = System.Math.Floor(cost);
        return usesMoney ? $"${flooredCost}" : $"{flooredCost} Apples";
    }
}