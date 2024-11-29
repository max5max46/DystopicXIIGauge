using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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

        enemyGSText.text = "+" + string.Format(CultureInfo.InvariantCulture, "{0:N0}", player.geometricScrapInRun);
        waveGSText.text = "+" + string.Format(CultureInfo.InvariantCulture, "{0:N0}", (wavesComplete * gsPerWave));
        walletGSText.text = string.Format(CultureInfo.InvariantCulture, "{0:N0}", player.geometricScrap);
        totalGSText.text = string.Format(CultureInfo.InvariantCulture, "{0:N0}", totalGS);
    }

}
