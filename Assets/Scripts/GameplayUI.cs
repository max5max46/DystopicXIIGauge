using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private GameObject waveProgressBar;
    [SerializeField] private TextMeshProUGUI gsText;
    [SerializeField] private GameObject reloadTimeIndicator;
    [SerializeField] private GameObject heart1;
    [SerializeField] private GameObject heart2;
    [SerializeField] private GameObject heart3;
    [SerializeField] private GameObject heart4;
    [SerializeField] private GameObject heart5;
    [SerializeField] private GameObject heart6;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private Shotgun shotgun;
    [SerializeField] private Player player;
    [SerializeField] private RoundManager roundManager;

    [Header("Sprite References")]
    [SerializeField] private Sprite reloadWheel1;
    [SerializeField] private Sprite reloadWheel2;
    [SerializeField] private Sprite reloadWheel3;
    [SerializeField] private Sprite reloadWheel4;

    [Header("Prefabs")]
    public GameObject shellPrefab;

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
            waveText.text = "Wave " + roundManager.waveManager.currentWave + "/" + roundManager.waveManager.wavePoints.Count;

            if (roundManager.waveManager.totalEnemiesInCurrentWave > 0)
                waveProgressBar.GetComponent<Image>().fillAmount = (1 - ((float)roundManager.waveManager.enemies.Count / (float)roundManager.waveManager.totalEnemiesInCurrentWave));
            else
                waveProgressBar.GetComponent<Image>().fillAmount = 0;
        }

        gsText.text = string.Format(CultureInfo.InvariantCulture, "{0:N0}", player.geometricScrapInRun);

        for (int i = 0; i < player.maxHealth; i++)
            hearts[i].GetComponent<Pip>().TurnPipOn();

        if (player.maxHealth > player.health)
        {
            for (int i = 0; i < (player.maxHealth - player.health); i++)
            {
                hearts[i + player.health].GetComponent<Pip>().TurnPipOff();
            }
        }

        if (shotgun.reloading)
            reloadTimeIndicator.GetComponent<Image>().fillAmount = Mathf.Clamp(shotgun.reloadTimer / shotgun.reloadTime, 0, 1);
        else
            reloadTimeIndicator.GetComponent<Image>().fillAmount = 0;

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

        reloadTimeIndicator.GetComponent<RectTransform>().localPosition = new Vector2(disBetweenShells + 40, 75);

        for (int i = 0; i < shotgun.clipSize; i++)
        {
            GameObject shell = Instantiate(shellPrefab, this.gameObject.transform);
            shell.GetComponent<RectTransform>().localPosition = new Vector2(disBetweenShells, 75);
            shell.GetComponent<Pip>().TurnPipOn();
            shells.Add(shell);
            disBetweenShells -= 22;
        }

        switch (upgradeManager.sReloadTime.upgrade.currentUpgradeLevel)
        {
            case 0:
                reloadTimeIndicator.GetComponent<Image>().sprite = reloadWheel1;
                break;

            case 1:
                reloadTimeIndicator.GetComponent<Image>().sprite = reloadWheel2;
                break;

            case 2:
                reloadTimeIndicator.GetComponent<Image>().sprite = reloadWheel3;
                break;

            case 3:
                reloadTimeIndicator.GetComponent<Image>().sprite = reloadWheel4;
                break;
        }
    }
}
