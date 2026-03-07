using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject moneyTutorialPanel;
    public GameObject appleTutorialPanel;

    public float autoHideDelay = 3.5f;

    public void ShowMoneyTutorial()
    {
        if (moneyTutorialPanel == null) return;
        
        moneyTutorialPanel.SetActive(true);
        Invoke(nameof(CloseMoneyTutorial), autoHideDelay);
    }

    public void ShowAppleTutorial()
    {
        if (appleTutorialPanel == null) return;

        appleTutorialPanel.SetActive(true);
        Invoke(nameof(CloseAppleTutorial), autoHideDelay);
    }

    public void CloseMoneyTutorial()
    {
        if (moneyTutorialPanel != null)
            moneyTutorialPanel.SetActive(false);
    }

    public void CloseAppleTutorial()
    {
        if (appleTutorialPanel != null)
            appleTutorialPanel.SetActive(false);
    }

    void Update()
    {
    }
}