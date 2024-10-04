using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI clipText;

    [SerializeField] private Shotgun shotgun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        clipText.text = "Clip " + shotgun.shellsInClip + "/" + shotgun.clipSize;
    }
}
