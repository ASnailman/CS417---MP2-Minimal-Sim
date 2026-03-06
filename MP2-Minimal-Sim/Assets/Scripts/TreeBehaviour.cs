using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TreeBehaviour : MonoBehaviour
{
    public int TreeID = 0;
    private float growth = 0f;
    private float growthRate = 0.015f; 

    public GameObject[] apples;

    public Camera mainCamera;
    public Canvas TreeInfo;
    public TextMeshProUGUI GrowthPercentage;

    public Button waterButton;
    public Button harvestButton;

    public TextMeshProUGUI UICanvasMessageText;

     void Start()
    {
        TreeInfo.enabled = false; 
    }
    // Update is called once per frame
    void Update()
    {
        int GardenPlotNumbers = UpgradesManager.F_upgrades[0].level; // Each level of the first farming upgrade adds 1 garden plot
        if (TreeID < GardenPlotNumbers) // Only grow if this tree is within the
        {
            TreeInfo.enabled = true;
            Renderer treeRenderer = gameObject.GetComponent<Renderer>();
            treeRenderer.enabled = true; // Ensure the tree model is visible when unlocked
        }
        else
        {
            // If the tree is not yet unlocked, ensure it's not visible and disable interactions
            growth = 0f;
            TreeInfo.enabled = false;
            foreach (GameObject apple in apples)
            {
                apple.SetActive(false);
            }
            Renderer treeRenderer = gameObject.GetComponent<Renderer>();
            treeRenderer.enabled = false; // Hide the tree model until it's unlocked
            return;
        }
        float growthEffectiveness = UpgradesManager.F_upgrades[2].level * 0.05f + 1f;
        growth += growthRate * growthEffectiveness * Time.deltaTime;
        if (growth > 1f) growth = 1f; // Cap growth at 100%
        GrowthPercentage.text = $"{(growth * 100f):F1}%";
        ShowCanvas();
        // Activate apples based on growth
        for (int i = 0; i < apples.Length; i++)
        {
            if (growth >= 0.2 + i * 0.1f) 
            {
                apples[i].SetActive(true);
            }
            else
            {
                apples[i].SetActive(false);
            }
        }
        if (growth >= 0.75f)
        {
            harvestButton.interactable = true;
        }
        else
        {
            harvestButton.interactable = false;
        }
        if (growth == 1f && UpgradesManager.F_upgrades[2].level >= 20)
        {
            HarvestTree(); // Auto-harvest when fully grown and the upgrade is at least level 20
        }
    }

    void ShowCanvas()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 4f))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                TreeInfo.enabled = true;
            }
            else
            {
                TreeInfo.enabled = false;
            }
        }
        else
        {
            TreeInfo.enabled = false;
        }
    }

    public void WaterTree()
    {
        if (ResourceManager.Instance.water >= 10f)
        {
            ResourceManager.Instance.water -= 10f;
            float growthEffectiveness = UpgradesManager.F_upgrades[1].level * 0.1f + 1f;
            growth += 0.1f * growthEffectiveness;
            if (growth > 1f) growth = 1f; // Cap growth at 100%
            waterButton.interactable = false; // Disable the button immediately after clicking
            Invoke(nameof(EnableWaterButton), 0.5f); // Re-enable the button after 3 seconds
        }
        else
        {
            UICanvasMessageText.text = "Not enough water!";
            UICanvasMessageText.color = Color.red; 
            Invoke(nameof(ClearMessage), 1.0f); 
        }
    }

    public void HarvestTree()
    {
        float applesGained = (8 + UpgradesManager.F_upgrades[3].level) * growth * (1 + UpgradesManager.F_upgrades[3].level * 0.02f); 
        // Harvest effectiveness is influenced by the 4th farming upgrade and total upgrade levels, providing a boost to apple yield
        float HarvestEffectiveness = 1 + UpgradesManager.Instance.TotalUpgradeLevel * 0.005f * UpgradesManager.F_upgrades[4].level;
        applesGained *= HarvestEffectiveness;
        ResourceManager.Instance.totalApples += Mathf.FloorToInt(applesGained);
        growth = 0f; 
    }
    private void ClearMessage()
    {
        UICanvasMessageText.text = ""; 
    }
    private void EnableWaterButton()
    {
        waterButton.interactable = true; 
    }
}
