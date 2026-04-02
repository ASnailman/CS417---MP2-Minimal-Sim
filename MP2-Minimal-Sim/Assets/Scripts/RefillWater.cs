using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class RefillWater : MonoBehaviour
{   
    public AudioSource refillWaterSound;
    public AudioSource buttonActivatedSound;
    public HapticImpulsePlayer leftHaptic;
    public HapticImpulsePlayer rightHaptic;

    public float cooldown = 5f;
    private float remainingTime;
    private bool isCoolingDown = false;
    public Button refillButton;
    public TextMeshProUGUI buttonText;

    public void refill()
    {
        if (refillWaterSound != null) {
            refillWaterSound.volume = 0.5f;
            refillWaterSound.Play();
        }
        leftHaptic?.SendHapticImpulse(0.8f, 0.1f);
        rightHaptic?.SendHapticImpulse(0.8f, 0.1f);

        float waterCapacity = (UpgradesManager.F_upgrades[1].level * 0.1f + 1) * 100;
        ResourceManager.Instance.water = waterCapacity;

        if (refillButton != null)
        {
            isCoolingDown = true;
            remainingTime = cooldown;

            refillButton.interactable = false;
            buttonText.text = "Wait " + Mathf.CeilToInt(remainingTime) + "s";
        }
    }

    void Update()
    {
        if (!isCoolingDown) return;

        remainingTime -= Time.deltaTime;
        buttonText.text = "Wait " + Mathf.CeilToInt(remainingTime) + "s";

        if (remainingTime <= 0f)
        {
            isCoolingDown = false;

            if (refillWaterSound != null) {
                buttonActivatedSound.volume = 1f;
                buttonActivatedSound.Play();
            }

            refillButton.interactable = true;
            buttonText.text = "Refill Water";
        }
    }
}
