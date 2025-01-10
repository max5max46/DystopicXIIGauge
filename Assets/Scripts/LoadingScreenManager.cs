using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float slideTime;
    [SerializeField] private AnimationCurve curve;

    [Header("References")]
    public Image loadingBar;
    public GameObject buttonShield;
    [SerializeField] private ProgramManager programManager;
    [SerializeField] private UIManager uiManager;

    private float slideTimer;
    private float jitterTimer;
    private int sceneToChangeTo;

    private string uiMenuName;

    private bool isSlidingIn;
    private bool isSlidingOut;

    // Start is called before the first frame update
    void Start()
    {
        buttonShield.SetActive(false);

        isSlidingIn = false;
        isSlidingOut = false;

        uiMenuName = "";

        transform.localPosition = new Vector3(2200, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isSlidingIn)
        {
            if (slideTimer < 0)
            {
                isSlidingIn = false;
                programManager.LoadScene(sceneToChangeTo);
            }
            else
            {
                slideTimer -= Time.unscaledDeltaTime;
                transform.localPosition = new Vector3(Mathf.Lerp(2200, 0, curve.Evaluate(Mathf.Clamp(1 - (slideTimer / slideTime), 0, 1))), 0, 0);
            }
        }

        if (isSlidingOut)
        {
            if (jitterTimer < 0)
            {
                if (slideTimer < 0)
                {
                    isSlidingOut = false;
                }
                else
                {
                    slideTimer -= Time.unscaledDeltaTime;
                    transform.localPosition = new Vector3(Mathf.Lerp(0, -2200, curve.Evaluate(Mathf.Clamp(1 - (slideTimer / slideTime), 0, 1))), 0, 0);
                }
            }
            else
            {
                jitterTimer -= Time.unscaledDeltaTime;
            }
        }
    }

    public void StartSlideIn(int sceneIndex)
    {
        loadingBar.fillAmount = 0;
        slideTimer = slideTime;
        buttonShield.SetActive(true);
        isSlidingIn = true;
        transform.position = new Vector3(2200, 0, 0);
        sceneToChangeTo = sceneIndex;
    }

    public void StartSlideOut()
    {
        loadingBar.fillAmount = 1;
        uiManager.SwitchUIScreen(uiMenuName);
        uiMenuName = "";
        slideTimer = slideTime;
        buttonShield.SetActive(false);
        jitterTimer = 0.5f;
        isSlidingOut = true;
    }

    public void SwitchUIScreenBetweenScene(string name)
    {
        uiMenuName = name;
    }
}
