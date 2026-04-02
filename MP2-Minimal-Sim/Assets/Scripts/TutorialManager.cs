using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject moneyTutorialPanel;
    public GameObject appleTutorialPanel;

    public EaseIn moneyTutorialEase;
    public EaseIn appleTutorialEase;

    public AudioSource tutorialSound;

    public float autoHideDelay = 3.5f;

    public void ShowMoneyTutorial()
    {
        if (moneyTutorialPanel == null) return;

        CancelInvoke(nameof(CloseMoneyTutorial));

        moneyTutorialPanel.SetActive(true);

        if (moneyTutorialEase != null)
            moneyTutorialEase.Pulse();

        if (tutorialSound != null)
            tutorialSound.Play();

        Invoke(nameof(CloseMoneyTutorial), autoHideDelay);
    }

    public void ShowAppleTutorial()
    {
        if (appleTutorialPanel == null) return;

        CancelInvoke(nameof(CloseAppleTutorial));

        appleTutorialPanel.SetActive(true);

        if (appleTutorialEase != null)
            appleTutorialEase.Pulse();

        if (tutorialSound != null)
            tutorialSound.Play();

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
        if (Input.anyKeyDown)
        {
            if (moneyTutorialPanel != null && moneyTutorialPanel.activeSelf)
            {
                CancelInvoke(nameof(CloseMoneyTutorial));
                CloseMoneyTutorial();
            }

            if (appleTutorialPanel != null && appleTutorialPanel.activeSelf)
            {
                CancelInvoke(nameof(CloseAppleTutorial));
                CloseAppleTutorial();
            }
        }   
    }
}