using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private GameObject waveProgressBar;
    [SerializeField] private TextMeshProUGUI gsText;
    [SerializeField] private GameObject heart1;
    [SerializeField] private GameObject heart2;
    [SerializeField] private GameObject heart3;
    [SerializeField] private GameObject heart4;
    [SerializeField] private GameObject heart5;
    [SerializeField] private GameObject heart6;
    //[SerializeField] private WaveManager waveManager;
    [SerializeField] private Shotgun shotgun;
    [SerializeField] private Player player;
    [SerializeField] private RoundManager roundManager;

    [Header("Prefabs")]
    [SerializeField] private GameObject shellPrefab;

    private List<GameObject> hearts;
    private List<GameObject> shells;

    // Start is called before the first frame update
    void Start()
    {
        ResetUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (roundManager.waveManager)
        {
            waveText.text = "Wave " + roundManager.waveManager.currentWave + "/" + roundManager.waveManager.amountOfWaves;
        }

        waveProgressBar.GetComponent<Image>().fillAmount = 1; //fill

        gsText.text = "GS: " + player.geometricScrapInRun;

        if (player.maxHealth > player.health)
        {
            for (int i = 0; i < (player.maxHealth - player.health); i++)
            {
                hearts[i + player.health].GetComponent<Pip>().TurnPipOff();
            }
        }

        foreach (GameObject shell in shells)
            shell.GetComponent<Pip>().TurnPipOn();

        for (int i = 0; i < (shotgun.clipSize - shotgun.shellsInClip); i++)
        {
            shells[i + shotgun.shellsInClip].GetComponent<Pip>().TurnPipOff();
        }
    }

    public void ResetUI()
    {
        hearts = new List<GameObject>();

        hearts.Add(heart1);
        hearts.Add(heart2);
        hearts.Add(heart3);
        hearts.Add(heart4);
        hearts.Add(heart5);
        hearts.Add(heart6);

        foreach (GameObject heart in hearts)
            heart.SetActive(false);

        for (int i = 0; i < player.maxHealth; i++)
        {
            Debug.Log(i);
            hearts[i].SetActive(true);
            hearts[i].GetComponent<Pip>().TurnPipOn();
        }

        if (shells != null)
            foreach (GameObject shell in shells)
                Destroy(shell);

        shells = new List<GameObject>();

        int disBetweenShells;

        if (shotgun.clipSize % 2 == 0)
            disBetweenShells = ((shotgun.clipSize / 2) * 22) - 11;
        else
            disBetweenShells = (shotgun.clipSize / 2) * 22;

        for (int i = 0; i < shotgun.clipSize; i++)
        {
            GameObject shell = Instantiate(shellPrefab, this.gameObject.transform);
            shell.GetComponent<RectTransform>().localPosition = new Vector2(disBetweenShells, 75);
            shell.GetComponent<Pip>().TurnPipOn();
            shells.Add(shell);
            disBetweenShells -= 22;
        }
    }
}
