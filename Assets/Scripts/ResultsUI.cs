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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        totalGS = player.geometricScrapInRun + player.geometricScrap + 100;

        enemyGSText.text = ("+" + player.geometricScrapInRun);
        waveGSText.text = ("+" + "100"); //FILL
        walletGSText.text = ("+" + player.geometricScrap);
        totalGSText.text = ("+" + (totalGS));
    }

    public void GivePlayerGS()
    {
        player.geometricScrap = totalGS;
    }
}
