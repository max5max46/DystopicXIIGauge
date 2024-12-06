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
    [SerializeField] private SpriteRenderer sprite;
    public GameObject pelletPrefab;

    [Header("Sprite References")]
    [SerializeField] private Sprite shotgunRight;
    [SerializeField] private Sprite shotgunLeft;

    [Header("Sound References")]
    [SerializeField] private SoundHandler soundHandler;
    [SerializeField] private AudioClip shotgunShot;
    [SerializeField] private AudioClip shotgunReload;
    [SerializeField] private AudioClip shotgunPump;


    [HideInInspector] public int shellsInClip;

    private Vector2 cursorDirection;
    private float shotCooldownTimer;
    [HideInInspector] public float reloadTimer;
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

        SpriteHandler();

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
            if (!reloading)
            {
                reloadTimer = reloadTime;
                reloading = true;
            }
            return;
        }

        if (shotCooldownTimer > 0)
            return;

        if (reloading == true && shellsInClip > 0)
            reloading = false;



        bool isFirstPellet = true;

        for (int i = 0; i < pelletAmount; i++)
        {
            GameObject pellet = Instantiate(pelletPrefab);
            pellet.GetComponent<Pellet>().damage = gunDamage;
            pellet.transform.position = transform.position;
            if (isFirstPellet)
            {
                pellet.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
                isFirstPellet = false;
            }
            else
                pellet.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, pelletSpread) + (transform.rotation.eulerAngles.z - (pelletSpread / 2)));
        }

        soundHandler.PlaySound(shotgunShot, 0.3f, transform.position);

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
        soundHandler.PlaySound(shotgunReload, 0.5f, transform.position);

        if (shellsInClip > clipSize)
            shellsInClip = clipSize;

        reloadTimer = reloadTime;
    }

    private void SpriteHandler()
    {
        bool isFacingRight = true;

        if (transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z < 270)
            isFacingRight = false;

        if (isFacingRight)
            sprite.sprite = shotgunRight;
        else
            sprite.sprite = shotgunLeft;

    }

    public void DRSReload()
    {
        if (reloading)
            reloading = false;

        shellsInClip = clipSize;
    }
}
