using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePip : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject onImage;
    [SerializeField] private GameObject offImage;

    // Start is called before the first frame update
    void Start()
    {
        offImage.SetActive(true);
        onImage.SetActive(false);
    }

    public void TurnPipOn()
    {
        offImage.SetActive(false);
        onImage.SetActive(true);
    }
}
