using UnityEngine;
using TMPro;

public class AchievementManager : MonoBehaviour
{
    public GameObject achievementPopupPanel;
    public TextMeshProUGUI achievementPopupText;
    public TextMeshProUGUI achievementListText;

    public int moneyGoal = 1000000;          
    public int appleGoal = 1000000;          
    public int winUpgradeGoal = 10;       
    public float speedrunTimeLimit = 300f; 

    private float startTime;
    private bool millionaireUnlocked = false;
    private bool appleFanaticUnlocked = false;
    private bool appleObsessionUnlocked = false;

    private string achievementList = "";

    void Start()
    {
        startTime = Time.time;
        if (achievementPopupPanel != null)
            achievementPopupPanel.SetActive(false);

        if (achievementListText != null)
            achievementListText.text = "";
    }

    void Update()
    {
        // Safety Check: If Managers aren't ready yet, skip this frame to prevent crashing
        if (ResourceManager.Instance == null || UpgradesManager.Instance == null) 
            return;

        CheckMillionaire();
        CheckAppleFanatic();
        CheckAppleObsession();
    }

    void CheckMillionaire()
    {
        // Now safe because of the null check in Update
        if (!millionaireUnlocked && ResourceManager.Instance.totalMoney >= moneyGoal)
        {
            millionaireUnlocked = true;
            UnlockAchievement("Millionaire");
        }
    }

    void CheckAppleFanatic()
    {
        if (!appleFanaticUnlocked && ResourceManager.Instance.totalApples >= appleGoal)
        {
            appleFanaticUnlocked = true;
            UnlockAchievement("Apple Fanatic");
        }
    }

    void CheckAppleObsession()
    {
        float elapsed = Time.time - startTime;

        if (!appleObsessionUnlocked && 
            elapsed <= speedrunTimeLimit && appleFanaticUnlocked &&
            UpgradesManager.Instance.TotalUpgradeLevel >= winUpgradeGoal)
        {
            appleObsessionUnlocked = true;
            UnlockAchievement("Apple Obsession");
        }
    }

    void UnlockAchievement(string achievementName)
    {
        ShowPopup(achievementName);
        AddToAchievementList(achievementName);
    }

    void ShowPopup(string achievementName)
    {
        if (achievementPopupPanel != null)
            achievementPopupPanel.SetActive(true);

        if (achievementPopupText != null)
            achievementPopupText.text = "Achievement Unlocked!\n" + achievementName;

        CancelInvoke(nameof(HidePopup));
        Invoke(nameof(HidePopup), 3f);
    }

    void HidePopup()
    {
        if (achievementPopupPanel != null)
            achievementPopupPanel.SetActive(false);
    }

    void AddToAchievementList(string achievementName)
    {
        if (achievementList.Contains(achievementName)) return;

        if (achievementList == "")
            achievementList = "- " + achievementName;
        else
            achievementList += "\n- " + achievementName;

        if (achievementListText != null)
            achievementListText.text = achievementList;
    }
}