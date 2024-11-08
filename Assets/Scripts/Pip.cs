using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pip : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject onImage;
    [SerializeField] private GameObject offImage;

    // Start is called before the first frame update
    void Start()
    {
        offImage.SetActive(false);
        onImage.SetActive(false);
    }

    public void TurnPipOn()
    {
        offImage.SetActive(false);
        onImage.SetActive(true);
    }

    public void TurnPipOff()
    {
        offImage.SetActive(true);
        onImage.SetActive(false);
    }
}
