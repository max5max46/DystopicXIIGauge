using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [Header("Properties")]
    public int gunDamage = 1;
    [SerializeField] private float shotCooldown = 0.3f;
    public int pelletAmount = 10;
    [SerializeField] private float pelletSpread = 15;
    public int clipSize = 3;
    public int amountOfShellsToReload = 1;
    public float reloadTime = 0.5f;
    [SerializeField] private float shotgunDisFromPlayer = 1.5f;

    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject pelletPrefab;

    [HideInInspector] public int shellsInClip;

    private Vector2 cursorDirection;
    private float shotCooldownTimer;
    private float reloadTimer;
    [HideInInspector] public bool reloading;

    // Start is called before the first frame update
    void Start()
    {
        shellsInClip = clipSize;
        shotCooldownTimer = 0;
        reloadTimer = 0;
        reloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        cursorDirection = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.GetComponent<Rigidbody2D>().position).normalized;

        transform.position = player.GetComponent<Rigidbody2D>().position + (cursorDirection * shotgunDisFromPlayer);

        float angle = Mathf.Atan2(cursorDirection.y, cursorDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (shotCooldownTimer > 0)
            shotCooldownTimer -= Time.deltaTime;

        if (reloadTimer > 0)
            reloadTimer -= Time.deltaTime;

        if (reloading)
            Reload();
    }

    public void Fire()
    {
        if (shellsInClip == 0)
        {
            reloadTimer = reloadTime;
            reloading = true;
            return;
        }

        if (shotCooldownTimer > 0)
            return;

        if (reloading == true && shellsInClip > 0)
            reloading = false;

        for (int i = 0; i < pelletAmount; i++)
        {
            GameObject pellet = Instantiate(pelletPrefab);
            pellet.transform.position = transform.position;
            pellet.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, pelletSpread) + (transform.rotation.eulerAngles.z - (pelletSpread/2)));
        }

        shotCooldownTimer = shotCooldown;
        shellsInClip--;
    }

    public void Reload()
    {
        if (reloading == false)
        {
            reloadTimer = reloadTime;
            reloading = true;
        }

        if (shellsInClip == clipSize)
        {
            reloading = false;
            return;
        }

        if (reloadTimer > 0)
        {
            return;
        }

        shellsInClip += amountOfShellsToReload;
        reloadTimer = reloadTime;
    }
}
