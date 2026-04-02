using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using System.Collections;

public class TreeBehaviour : MonoBehaviour
{
    public int TreeID = 0;
    private float growth = 0f;
    private float growthRate = 0.015f; 

    public GameObject[] apples;
    public Camera mainCamera; // Optional now, but kept to avoid inspector errors
    public Canvas TreeInfo;
    public TextMeshProUGUI GrowthPercentage;

    public Button waterButton;
    public Button harvestButton;

    public ParticleSystem appleParticles;

    public AudioSource waterSound;
    public AudioSource harvestSound;
    public HapticImpulsePlayer leftHaptic;
    public HapticImpulsePlayer rightHaptic;

    public TextMeshProUGUI UICanvasMessageText;
    public bool isUnlocked = false;

    void Start()
    {
        // Initialization handled in Update to ensure sync with UpgradesManager
    }

    void Update()
    {
        // 1. Check if UpgradesManager is ready to avoid crashes
        if (UpgradesManager.F_upgrades == null || UpgradesManager.F_upgrades.Count == 0) return;

        // 2. Determine if this plot is unlocked
        int GardenPlotNumbers = UpgradesManager.F_upgrades[0].level;
        isUnlocked = TreeID < GardenPlotNumbers;

        TreeInfo.enabled = isUnlocked;
        Renderer treeRenderer = GetComponent<Renderer>();
        if (treeRenderer != null) treeRenderer.enabled = isUnlocked;

        // Apply visibility
        // if (TreeID == 1) {
        // }
        // else if (TreeID == 2) {
        //     if (ResourceManager.Instance.tree2 != null)
        //     {
        //         ResourceManager.Instance.tree2.Pulse();
        //     }
        // }
        // else if (TreeID == 3) {
        //     if (ResourceManager.Instance.tree3 != null)
        //     {
        //         ResourceManager.Instance.tree3.Pulse();
        //     }
        // }
        // else if (TreeID == 4) {
        //     if (ResourceManager.Instance.tree4 != null)
        //     {
        //         ResourceManager.Instance.tree4.Pulse();
        //     }
        // }
        // else if (TreeID == 5) {
        //     if (ResourceManager.Instance.tree5 != null)
        //     {
        //         ResourceManager.Instance.tree5.Pulse();
        //     }
        // }

        if (!isUnlocked)
        {
            growth = 0f;
            foreach (GameObject apple in apples) apple.SetActive(false);
            
            return;
        }

        // 3. Growth Logic
        float growthEffectiveness = UpgradesManager.F_upgrades[2].level * 0.05f + 1f;
        growth += growthRate * growthEffectiveness * Time.deltaTime;
        growth = Mathf.Clamp01(growth); // Cap at 1.0 (100%)

        GrowthPercentage.text = $"{(growth * 100f):F1}%";

        // 4. Update Apple Visuals
        for (int i = 0; i < apples.Length; i++)
        {
            apples[i].SetActive(growth >= 0.2f + (i * 0.1f));
            ResourceManager.Instance.tree1.Pulse();
        }

        // 5. Button Logic
        harvestButton.interactable = (growth >= 0.75f);

        // Auto-harvest at max upgrade level
        if (growth >= 1f && UpgradesManager.F_upgrades[2].level >= 20)
        {
            HarvestTree();
        }
    }

    public void WaterTree()
    {
        if (ResourceManager.Instance == null) return;

        if (ResourceManager.Instance.water >= 10f)
        {
            leftHaptic?.SendHapticImpulse(0.5f, 0.1f);
            rightHaptic?.SendHapticImpulse(0.5f, 0.1f);

            ResourceManager.Instance.water -= 10f;
            float growthEffectiveness = UpgradesManager.F_upgrades[1].level * 0.1f + 1f;
            growth = Mathf.Clamp01(growth + (0.1f * growthEffectiveness));
            
            waterButton.interactable = false;
            Invoke(nameof(EnableWaterButton), 0.5f);
            if(waterSound != null)
            {
                waterSound.transform.position = transform.position; // Move sound to tree position
                waterSound.volume = 1f;
                waterSound.Play();
            }
        }
        else
        {
            if (UICanvasMessageText != null)
            {
                UICanvasMessageText.text = "Not enough water!";
                UICanvasMessageText.color = Color.red; 
                Invoke(nameof(ClearMessage), 1.0f); 
            }
        }
    }

    public void HarvestTree()
    {
        if (ResourceManager.Instance == null) return;

        if (harvestSound != null) {
            harvestSound.transform.position = transform.position; // Move sound to tree position
            harvestSound.volume = 1f;
            harvestSound.Play();
        }
        leftHaptic?.SendHapticImpulse(0.5f, 0.1f);
        rightHaptic?.SendHapticImpulse(0.5f, 0.1f);

        float applesGained = (8 + UpgradesManager.F_upgrades[3].level) * growth * (1 + UpgradesManager.F_upgrades[3].level * 0.02f); 
        float harvestEffectiveness = 1 + UpgradesManager.Instance.TotalUpgradeLevel * 0.005f * UpgradesManager.F_upgrades[4].level;
        
        ResourceManager.Instance.totalApples += Mathf.FloorToInt(applesGained * harvestEffectiveness);
        growth = 0f; 

        if (appleParticles != null)
        {
            appleParticles.transform.position = transform.position;
            appleParticles.Emit(1);
        }
    }

    private void ClearMessage() => UICanvasMessageText.text = "";
    private void EnableWaterButton() => waterButton.interactable = true;

    void Delay() {
        TreeInfo.enabled = isUnlocked;
    }
}