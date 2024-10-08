using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private int gunDamage = 1;
    [SerializeField] private float shotCooldown = 0.3f;
    [SerializeField] private int pelletAmount = 10;
    [SerializeField] private float pelletSpread = 15;
    [SerializeField] private float reloadTime = 0.5f;
    public int clipSize = 3;
    [SerializeField] private float shotgunDisFromPlayer = 1.5f;

    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject pelletPrefab;

    [HideInInspector] public int shellsInClip;

    private Vector2 cursorDirection;

    // Start is called before the first frame update
    void Start()
    {
        shellsInClip = clipSize;
    }

    // Update is called once per frame
    void Update()
    {
        cursorDirection = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.GetComponent<Rigidbody2D>().position).normalized;

        transform.position = player.GetComponent<Rigidbody2D>().position + (cursorDirection * shotgunDisFromPlayer);

        float angle = Mathf.Atan2(cursorDirection.y, cursorDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Fire()
    {
        if (shellsInClip == 0)
            return;

        for (int i = 0; i < pelletAmount; i++)
        {
            GameObject pellet = Instantiate(pelletPrefab);
            pellet.transform.position = transform.position;
            pellet.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, pelletSpread) + (transform.rotation.eulerAngles.z - (pelletSpread/2)));
        }

        shellsInClip--;
    }

    public void Reload()
    {
        if (shellsInClip == clipSize)
            return;

        shellsInClip++;
    }
}
