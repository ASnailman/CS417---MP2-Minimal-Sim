using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class RefillWater : MonoBehaviour
{   
    public AudioSource refillWaterSound;
    public HapticImpulsePlayer leftHaptic;
    public HapticImpulsePlayer rightHaptic;

    public void refill()
    {
        if (refillWaterSound != null) {
            refillWaterSound.volume = 0.5f;
            refillWaterSound.Play();
        }
        leftHaptic?.SendHapticImpulse(0.5f, 0.1f);
        rightHaptic?.SendHapticImpulse(0.5f, 0.1f);

        float waterCapacity = (UpgradesManager.F_upgrades[1].level * 0.1f + 1) * 100;
        ResourceManager.Instance.water = waterCapacity;
    }
}
