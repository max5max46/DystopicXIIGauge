using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamAttackVisual : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float aliveTime;
    [SerializeField] private float fadeTime;

    [Header("References")]
    [SerializeField] private SpriteRenderer sprite;

    private float aliveTimer;
    private float fadeTimer;
    private float startingAlpha;

    // Start is called before the first frame update
    void Start()
    {
        fadeTimer = 0;
        aliveTimer = aliveTime;
        startingAlpha = sprite.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        aliveTimer -= Time.deltaTime;
        
        if (aliveTimer < 0)
        {
            fadeTimer += Time.deltaTime;
            float percentageComplete = fadeTimer / fadeTime;

            Color temp = sprite.color;
            temp.a = Mathf.Lerp(startingAlpha, 0, percentageComplete);

            sprite.color = temp;
        }

        if (fadeTimer > fadeTime)
        {
            Destroy(this.gameObject);
        }
    }
}
