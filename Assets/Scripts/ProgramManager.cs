using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgramManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
