using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI clipText;
    [SerializeField] private TextMeshProUGUI partsText;
    [SerializeField] private TextMeshProUGUI healthText;
    //[SerializeField] private WaveManager waveManager;
    [SerializeField] private Shotgun shotgun;
    [SerializeField] private Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //waveText.text = "Wave " + waveManager.currentWave + "/" + waveManager.amountOfWaves;
        clipText.text = "Clip " + shotgun.shellsInClip + "/" + shotgun.clipSize;
        partsText.text = "GS: " + player.parts;
        healthText.text = "Health " + player.health + "/" + player.maxHealth;
    }
}
