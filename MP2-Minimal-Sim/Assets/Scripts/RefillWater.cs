using UnityEngine;
public class RefillWater : MonoBehaviour
{

    public void refill()
    {
        float waterCapacity = (UpgradesManager.M_upgrades[1].level * 0.1f + 1) * 100;
        ResourceManager.Instance.water = waterCapacity;
    }
}
