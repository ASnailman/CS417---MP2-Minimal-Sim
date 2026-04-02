using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics; 

public class MoneyGenerator : MonoBehaviour
{
    public double baserate = 1.0;
    private float particles = 0f;
    public ParticleSystem moneyParticles;
    public AudioSource moneySound;
    public HapticImpulsePlayer leftHaptic;
    public HapticImpulsePlayer rightHaptic;

    public double getCurrentRate() //
    {
        // Safety check: Ensure UpgradesManager has populated the list
        if (UpgradesManager.M_upgrades == null || UpgradesManager.M_upgrades.Count < 4)
        {
            return baserate;
        }

        double currentRate = baserate;
        currentRate += UpgradesManager.M_upgrades[0].level * 1.0; 
        currentRate *= 1 + UpgradesManager.M_upgrades[2].level * 0.02;
        currentRate *= 1 + (UpgradesManager.M_upgrades[3].level * 0.04 * ResourceManager.Instance.totalApples * 0.01); 
        return currentRate;
    }

    void Update()
    {
        if (ResourceManager.Instance == null) return;
        double moneyGained = getCurrentRate() * Time.deltaTime;
        ResourceManager.Instance.totalMoney += moneyGained;

        particles += (float) moneyGained;
        if (particles >= 1.0f) {
            TriggerParticle();
            particles = 0f;
            if (ResourceManager.Instance.moneyTextEase != null)
            {
                ResourceManager.Instance.moneyTextEase.Pulse();
            }
        }

    }

    void TriggerParticle()
    {
        if (moneyParticles != null) {
            moneyParticles.Emit(1);
        }
        if (moneySound != null) {
            moneySound.volume = 0.2f;
            moneySound.Play();
        }
        
        leftHaptic?.SendHapticImpulse(0.5f, 0.1f);
        rightHaptic?.SendHapticImpulse(0.5f, 0.1f);
    }
}