using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class MiningGainMoney : MonoBehaviour
{
    public static MiningGainMoney Instance;
    public double baseRate = 1.0;

    public TextMeshProUGUI UICanvasMessageText;
    public Button mineButton;
    public ParticleSystem moneyParticles;
    public AudioSource moneySound;
    public HapticImpulsePlayer leftHaptic;
    public HapticImpulsePlayer rightHaptic;
    private float particles = 0f;

    private void Awake()
    {
        Instance = this;
    }

    public static double getCurrentRate()
    {
        if (Instance == null) return 1.0;

        // Safety check: Ensure UpgradesManager list exists and has enough entries
        if (UpgradesManager.M_upgrades == null || UpgradesManager.M_upgrades.Count < 4 || ResourceManager.Instance == null) 
        {
            return Instance.baseRate;
        }

        double currentRate = Instance.baseRate;
        currentRate += UpgradesManager.M_upgrades[1].level * 10;
        currentRate *= 1 + UpgradesManager.M_upgrades[2].level * 0.02;
        currentRate *= 1 + (UpgradesManager.M_upgrades[3].level * 0.04 * ResourceManager.Instance.totalApples * 0.01);
        if (MoneySurge.Instance != null)
        {
            currentRate *= MoneySurge.Instance.GetCurrentMultiplier();
        }

        return currentRate;
    }

    public void Mine()
    {
        // Guard against clicking before lists are ready
        if (UpgradesManager.M_upgrades == null || UpgradesManager.M_upgrades.Count < 4) return;

        double moneyGained = getCurrentRate();
        float DoubleDrop = Random.Range(0f, 1f);
        bool flgDoubleDrop = false;
        
        if (DoubleDrop <= UpgradesManager.M_upgrades[2].level * 0.02f)
        {
            moneyGained *= 2; 
            flgDoubleDrop = true;
        }

        ResourceManager.Instance.totalMoney += moneyGained;
        TriggerParticle();
        if (ResourceManager.Instance.mineTextEase != null)
        {
            ResourceManager.Instance.mineTextEase.Pulse();
        }
        mineButton.interactable = false; 
        Invoke(nameof(EnableMineButton), 0.5f);

        if (flgDoubleDrop)
        {
            UICanvasMessageText.text = "Double Drop!";
            UICanvasMessageText.color = Color.yellow;
            Invoke(nameof(ClearMessage), 1.0f);
        }
    }

    void TriggerParticle()
    {
        if (moneyParticles != null) {
            moneyParticles.Emit(1);
        }
        if (moneySound != null) {
            moneySound.volume = 1f;
            moneySound.Play();
        }
        leftHaptic?.SendHapticImpulse(0.5f, 0.1f);
        rightHaptic?.SendHapticImpulse(0.5f, 0.1f);
    }

    private void EnableMineButton() => mineButton.interactable = true;
    private void ClearMessage() => UICanvasMessageText.text = "";

    void Update()
    {
        // SAFETY SHIELD: Stop UI update if dependencies aren't ready
        if (mineButton == null || ResourceManager.Instance == null || UpgradesManager.M_upgrades.Count < 4)
        {
            return;
        }

        var txt = mineButton.transform.Find("MiningAreaBtnTxt").GetComponent<TextMeshProUGUI>();
        if (txt != null) 
        {
            txt.text = $"+${System.Math.Floor(getCurrentRate())}";
        }
    }
}