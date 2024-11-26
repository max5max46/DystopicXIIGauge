using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgramManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player player;

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void SwitchSceneToGameplay()
    {
        SceneManager.LoadScene("Gameplay");
        player.transform.position = Vector3.zero;
    }

    public void SwitchSceneToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
