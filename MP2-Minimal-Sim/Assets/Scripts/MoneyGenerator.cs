using UnityEngine;
using TMPro;

public class MoneyGenerator : MonoBehaviour
{

    public float rate = 1.0f;

    // Update is called once per frame
    void Update()
    {
        float moneyGained = rate * Time.deltaTime;
        ResourceManager.Instance.totalMoney += moneyGained;
    }
}
