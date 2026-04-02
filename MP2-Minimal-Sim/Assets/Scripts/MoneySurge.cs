using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class MoneySurge : MonoBehaviour
{
    public static MoneySurge Instance;

    public GameObject TreasureChest;
    public GameObject spawnLocation;

    public TextMeshProUGUI UIMsgText;
    public Animator animator;
    public double spawnRangeX = 2.0;
    public double spawnRangeZ = 2.0;
    public double surgeDuration = 30.0;
    public double surgeMultiplier = 2.0;
    public float closeDespawnDelay = 1.0f;
    private double surgeEndTime = 0.0;
    public double spawnCooldown = 60.0;
    private double nextSpawnTime = 0.0;
    private bool surgeActive = false;
    private bool surgeSpawned = false;
    private XRGrabInteractable chestGrabInteractable;

    void Start()
    {
        Instance = this;
        nextSpawnTime = spawnCooldown;

        if (TreasureChest != null)
        {
            chestGrabInteractable = TreasureChest.GetComponent<XRGrabInteractable>();
            if (chestGrabInteractable != null)
            {
                chestGrabInteractable.selectEntered.AddListener(OnChestGrabbed);
            }
        }
    }

    private void OnDestroy()
    {
        if (chestGrabInteractable != null)
        {
            chestGrabInteractable.selectEntered.RemoveListener(OnChestGrabbed);
        }
    }

    private void OnChestGrabbed(SelectEnterEventArgs args)
    {
        ActivateSurge();
    }
    void Update()
    {
        if(!surgeActive && nextSpawnTime > 0 && !surgeSpawned)
        {
            nextSpawnTime -= Time.deltaTime;
            if (nextSpawnTime <= 0)
            {
                surgeSpawned = true;
                SpawnSurge();
                Debug.Log("Money Surge Spawned!");
            }
        }
        else if(surgeActive && surgeEndTime > 0)
        {
            surgeEndTime -= Time.deltaTime;
            if(surgeEndTime <= 0)
            {
                surgeActive = false;
                nextSpawnTime = spawnCooldown;
                CloseAndDespawnChest();
                Debug.Log("Money Surge Ended!");
                if (UIMsgText != null)
                {
                    UIMsgText.text = "";
                }
            }
        }
    }

    public void ActivateSurge()
    {
        if (!surgeSpawned || surgeActive)
        {
            return;
        }

        if (animator != null)
        {
            animator.SetBool("PlayOpen", true);
        }

        surgeActive = true;
        surgeEndTime = surgeDuration;
        surgeSpawned = false;

        if (UIMsgText != null)
        {
            UIMsgText.text = $"Money Surge x{surgeMultiplier}!";
        }

        Debug.Log($"Money Surge Activated for {surgeDuration}s at x{surgeMultiplier}.");
    }

    public double GetCurrentMultiplier()
    {
        return surgeActive ? surgeMultiplier : 1.0;
    }

    void SpawnSurge()
    {
        //Randomize spawn position within range
        Vector3 spawnPos = spawnLocation.transform.position;
        spawnPos.x += Random.Range((float)-spawnRangeX, (float)spawnRangeX);
        spawnPos.z += Random.Range((float)-spawnRangeZ, (float)spawnRangeZ);
        TreasureChest.transform.position = spawnPos;

        if (animator != null)
        {
            animator.SetBool("PlayOpen", false);
        }

        TreasureChest.SetActive(true);
    }

    private void CloseAndDespawnChest()
    {
        if (TreasureChest == null)
        {
            return;
        }

        if (animator != null)
        {
            animator.SetBool("PlayOpen", false);
        }

        StartCoroutine(DespawnChestAfterDelay());
    }

    private IEnumerator DespawnChestAfterDelay()
    {
        float delay = Mathf.Max(0f, closeDespawnDelay);
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        if (TreasureChest != null)
        {
            TreasureChest.SetActive(false);
        }
    }
    
}
