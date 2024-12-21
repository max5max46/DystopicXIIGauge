using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("UI Screen References")]
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

    [Header("Additional References")]
    [SerializeField] private Player player;
    [SerializeField] private ProgramManager programManager;
    [SerializeField] private MusicHandler musicHandler;
    [SerializeField] private Button clearSaveButton;

    private string previousUIScreenName;
    [HideInInspector] public string currentUIScreenName;


    // Start is called before the first frame update
    void Start()
    {
        previousUIScreenName = "main";
        currentUIScreenName = "main";

        SwitchUIScreen("main");
    }



    public void SwitchUIScreen(string name = "none")
    {
        bool resetUIScreens = true;

        if (name == "story" && programManager.DoesSaveExist())
            name = "upgrades";

        if (name == "pause" || name == "results")
        {
            player.canControl = false;
            programManager.PauseGame();
        }

        if (currentUIScreenName == "pause" && name == "pause")
            name = "gameplay";

        if (name == "upgrades" || name == "gameplay")
        {
            if (name == "gameplay")
                player.canControl = true;

            programManager.ResumeGame();
        }

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
                musicHandler.SwitchAudioTrack("title");
                musicHandler.RestoreVolume();
                break;

            case "warning":
                uiWarning.SetActive(true);
                musicHandler.SwitchAudioTrack("title");
                musicHandler.RestoreVolume();
                break;

            case "options":
                uiOptions.SetActive(true);
                break;

            case "story":
                uiStory.SetActive(true);
                musicHandler.SwitchAudioTrack("title");
                musicHandler.RestoreVolume();
                break;

            case "tutorial":
                uiTutorial.SetActive(true);
                musicHandler.SwitchAudioTrack("title");
                musicHandler.RestoreVolume();
                break;

            case "upgrades":
                uiUpgrades.SetActive(true);
                musicHandler.SwitchAudioTrack("shop");
                musicHandler.RestoreVolume();
                break;

            case "gameplay":
                uiGameplay.SetActive(true);
                musicHandler.SwitchAudioTrack("fight");
                musicHandler.RestoreVolume();
                break;

            case "pause":
                uiPause.SetActive(true);
                musicHandler.SwitchAudioTrack("fight");
                musicHandler.LowerVolume();
                break;

            case "results":
                uiResults.SetActive(true);
                musicHandler.SwitchAudioTrack("fight");
                musicHandler.LowerVolume();
                break;

            case "win":
                uiWin.SetActive(true);
                musicHandler.SwitchAudioTrack("title");
                musicHandler.RestoreVolume();
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

        // Handles whether or not the CLear Save Button should be grayed out
        if (programManager.DoesSaveExist())
            clearSaveButton.interactable = true;
        else
            clearSaveButton.interactable = false;
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
