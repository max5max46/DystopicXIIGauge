using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private GameObject uiMain;
    [SerializeField] private GameObject uiWarning;
    [SerializeField] private GameObject uiOptions;
    [SerializeField] private GameObject uiStory;
    [SerializeField] private GameObject uiTutorial;
    [SerializeField] private GameObject uiUpgrades;
    [SerializeField] private GameObject uiGameplay;
    [SerializeField] private GameObject uiPause;
    [SerializeField] private GameObject uiResults;
    [SerializeField] private GameObject uiWin;
    [SerializeField] private ProgramManager programManager;

    private string previousUIScreenName;
    [HideInInspector] public string currentUIScreenName;


    // Start is called before the first frame update
    void Start()
    {
        SwitchUIScreen("main");
    }

    public void SwitchUIScreen(string name = "none")
    {
        bool resetUIScreens = true;

        if (name == "story" && programManager.DoesSaveExist())
            name = "upgrades";

        if (name == "pause" || name == "results")
            programManager.PauseGame();

        if (currentUIScreenName == "pause" && name == "pause")
            name = "gameplay";

        if (name == "upgrades" || name == "gameplay")
            programManager.ResumeGame();

        if (name == "options" || name == "warning")
            resetUIScreens = false;

        if (currentUIScreenName == "options")
            name = previousUIScreenName;

        if (resetUIScreens)
            DeactivateUIScreens();

        switch (name)
        {
            case "main":
                uiMain.SetActive(true);
                uiUpgrades.SetActive(true);
                break;

            case "warning":
                uiWarning.SetActive(true);
                break;

            case "options":
                uiOptions.SetActive(true);
                break;

            case "story":
                uiStory.SetActive(true);
                break;

            case "tutorial":
                uiTutorial.SetActive(true);
                break;

            case "upgrades":
                uiUpgrades.SetActive(true);
                break;

            case "gameplay":
                uiGameplay.SetActive(true);
                break;

            case "pause":
                uiPause.SetActive(true);
                break;

            case "results":
                uiResults.SetActive(true);
                break;

            case "win":
                uiWin.SetActive(true);
                break;

            case "none":
                Debug.LogError("ERROR: No name given to switch UI screen");
                return;

            default:
                Debug.LogError("ERROR: Name given to switch UI screen Invaild");
                return;
        }

        previousUIScreenName = currentUIScreenName;
        currentUIScreenName = name;
    }


    private void DeactivateUIScreens()
    {
        uiMain.SetActive(false);
        uiWarning.SetActive(false);
        uiOptions.SetActive(false);
        uiStory.SetActive(false);
        uiTutorial.SetActive(false);
        uiUpgrades.SetActive(false);
        uiGameplay.SetActive(false);
        uiPause.SetActive(false);
        uiResults.SetActive(false);
        uiWin.SetActive(false);
    }

}
