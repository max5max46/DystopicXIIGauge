using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultsUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI enemyGSText;
    [SerializeField] private TextMeshProUGUI waveGSText;
    [SerializeField] private TextMeshProUGUI walletGSText;
    [SerializeField] private TextMeshProUGUI totalGSText;

    private int totalGS;

    // Update is called once per frame
    public void CalculateResults(int wavesComplete, int gsPerWave)
    {
        totalGS = player.geometricScrapInRun + player.geometricScrap + (wavesComplete * gsPerWave);

        enemyGSText.text = ("+" + player.geometricScrapInRun);
        waveGSText.text = ("+" + (wavesComplete * gsPerWave));
        walletGSText.text = ("+" + player.geometricScrap);
        totalGSText.text = ("+" + (totalGS));
    }

}
