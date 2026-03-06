using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject moneyTutorialPanel;
    public GameObject appleTutorialPanel;

    private bool waitForMouseRelease = false;

    public void ShowMoneyTutorial()
    {
        moneyTutorialPanel.SetActive(true);
        waitForMouseRelease = true;
    }

    public void ShowAppleTutorial()
    {
        appleTutorialPanel.SetActive(true);
        waitForMouseRelease = true;
    }

    public void CloseMoneyTutorial()
    {
        moneyTutorialPanel.SetActive(false);
    }

    public void CloseAppleTutorial()
    {
        appleTutorialPanel.SetActive(false);
    }

    void Update()
    {
        bool anyTutorialOpen =
            moneyTutorialPanel != null && moneyTutorialPanel.activeSelf ||
            appleTutorialPanel != null && appleTutorialPanel.activeSelf;

        if (!anyTutorialOpen) return;

      
        if (waitForMouseRelease)
        {
            if (!Input.GetMouseButton(0))
            {
                waitForMouseRelease = false;
            }
            return;
        }

        
        if (Input.GetMouseButtonDown(0))
        {
            if (moneyTutorialPanel != null) moneyTutorialPanel.SetActive(false);
            if (appleTutorialPanel != null) appleTutorialPanel.SetActive(false);
        }
    }
}
