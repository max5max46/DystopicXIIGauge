using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Properties")]
    public int maxHealth = 3;
    public float speedMultiplier = 1;
    [SerializeField] private float immunityTime = 0.5f;
    [SerializeField] private float maxSpeed = 5;
    [SerializeField] private float acceleration = 10;
    [SerializeField] private float decelerationMultiplier = 0.8f;
    public float reloadSpeedReduction = 0.5f;
    [SerializeField] private float edsRadious;
    [SerializeField] private int edsDamage;

    [Header("References")]
    [SerializeField] private Shotgun shotgun;
    [SerializeField] private GameObject explosionParticlePrefab;
    [SerializeField] private SpriteRenderer bodySprite;
    [SerializeField] private SpriteRenderer faceSprite;
    [SerializeField] private RoundManager roundManager;
    [SerializeField] private UIManager uiManager;

    [Header("Sprite References")]
    [SerializeField] private Sprite bodyRight;
    [SerializeField] private Sprite bodyLeft;
    [SerializeField] private Sprite faceRight;
    [SerializeField] private Sprite faceLeft;
    [SerializeField] private Sprite faceHurtRight;
    [SerializeField] private Sprite faceHurtLeft;

    [Header("Sound References")]
    [SerializeField] private SoundHandler soundHandler;
    [SerializeField] private AudioClip playerHitSound;
    [SerializeField] private AudioClip explosiveSound;

    [Header("Debug")]
    [SerializeField] private bool godMode = false;

    [HideInInspector] public bool isEDSActive;
    [HideInInspector] public bool isDRSActive;
    [HideInInspector] public int health;
    private Rigidbody2D rb;
    private Vector2 movementVector;
    private float currentReloadSpeedReduction;
    private float immunityTimer;
    private float flickerTimer;

    [HideInInspector] public int geometricScrapInRun;
    [HideInInspector] public int geometricScrap;
    [HideInInspector] public bool canControl;

    private bool upPressed;
    private bool downPressed;
    private bool rightPressed;
    private bool leftPressed;
    private bool firePressed;
    private bool reloadPressed;
    private bool pausePressed;

    // Start is called before the first frame update
    void Start()
    {
        isEDSActive = false;
        isDRSActive = false;
        geometricScrapInRun = 0;
        canControl = true;
        health = maxHealth;
        currentReloadSpeedReduction = 1;
        immunityTimer = 0;
        flickerTimer = 0;
        rb = GetComponent<Rigidbody2D>();

        ManageInputs(true);
    }

    // Update is called once per frame
    void Update()
    {
        ManageInputs();

        SetMovementVector();

        SpriteHandler();

        if (shotgun.reloading)
            currentReloadSpeedReduction = reloadSpeedReduction;
        else
            currentReloadSpeedReduction = 1;

        if (firePressed)
            shotgun.Fire();

        if (reloadPressed)
            shotgun.Reload();

        if (geometricScrap > 999999)
            geometricScrap = 999999;

        if (immunityTimer > 0)
            immunityTimer -= Time.deltaTime;

        if (pausePressed && (uiManager.currentUIScreenName == "gameplay" || uiManager.currentUIScreenName == "pause"))
            uiManager.SwitchUIScreen("pause");

        ManageInputs(true);
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < (maxSpeed * speedMultiplier) * currentReloadSpeedReduction)
            rb.AddRelativeForce(movementVector);

        // Increases deceleration to prevent sliding
        rb.velocity = new Vector2(rb.velocity.x * decelerationMultiplier, rb.velocity.y * decelerationMultiplier);
    }

    private void ManageInputs(bool resetInputs = false)
    {
        if (!resetInputs)
        {
            // Grabs player input and stores it
            if (canControl)
            {
                if (Input.GetKey(KeyCode.W))
                    upPressed = true;

                if (Input.GetKey(KeyCode.S))
                    downPressed = true;

                if (Input.GetKey(KeyCode.D))
                    rightPressed = true;

                if (Input.GetKey(KeyCode.A))
                    leftPressed = true;

                if (Input.GetKey(KeyCode.Mouse0))
                    firePressed = true;

                if (Input.GetKeyDown(KeyCode.Mouse1))
                    reloadPressed = true;

                if (Input.GetKeyDown(KeyCode.Escape))
                    pausePressed = true;
            }
        }
        else
        {
            upPressed = false;
            downPressed = false;
            rightPressed = false;
            leftPressed = false;
            firePressed = false;
            reloadPressed = false;
            pausePressed = false;
        }
    }

    private void SetMovementVector()
    {
        // Resets force adding variables to zero for the next add force
        float moveForwardBackward = 0;
        float moveLeftRight = 0;

        // Turns WASD input into the add force variables 
        if (upPressed)
            moveForwardBackward += 1;

        if (downPressed)
            moveForwardBackward -= 1;

        if (rightPressed)
            moveLeftRight += 1;

        if (leftPressed)
            moveLeftRight -= 1;

        movementVector = new Vector2(moveLeftRight, moveForwardBackward).normalized * acceleration * speedMultiplier;
    }

    public void TakeDamage(int damage)
    {
        if (health < 1 || immunityTimer > 0 || !canControl || godMode)
            return;

        health -= damage;
        immunityTimer = immunityTime;
        soundHandler.PlaySound(playerHitSound, 0.4f, transform.position);

        if (isDRSActive)
            shotgun.DRSReload();

        if (isEDSActive)
            ExplosiveDefenseSystem();

        if (health < 1)
            roundManager.RoundEnd(false);
    }

    private void SpriteHandler()
    {
        bool isFacingRight = true;

        if (shotgun.transform.rotation.eulerAngles.z > 90 && shotgun.transform.rotation.eulerAngles.z < 270)
            isFacingRight = false;

        if (isFacingRight)
            bodySprite.sprite = bodyRight;
        else
            bodySprite.sprite = bodyLeft;

        if (immunityTimer > 0)
        {
            if (isFacingRight)
                faceSprite.sprite = faceHurtRight;
            else
                faceSprite.sprite = faceHurtLeft;

            if (flickerTimer > 0)
            {
                flickerTimer -= Time.deltaTime;
            }
            else
            {
                if (faceSprite.enabled == true)
                {
                    bodySprite.enabled = false;
                    faceSprite.enabled = false;
                }
                else
                {
                    bodySprite.enabled = true;
                    faceSprite.enabled = true;
                }

                flickerTimer = immunityTime / 5f;
            }
        }
        else
        {
            if (isFacingRight)
                faceSprite.sprite = faceRight;
            else
                faceSprite.sprite = faceLeft;

            bodySprite.enabled = true;
            faceSprite.enabled = true;
            flickerTimer = 0;
        }
    }

    public void ReceivePartsInRun(int partsToReceive)
    {
        geometricScrapInRun += partsToReceive;
    }

    private void ExplosiveDefenseSystem()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, edsRadious);

        GameObject explosionParticle = Instantiate(explosionParticlePrefab, transform.position, transform.rotation);
        explosionParticle.GetComponent<OneTimeParticle>().StartParticles(null, edsRadious);

        soundHandler.PlaySound(explosiveSound, 0.5f, transform.position);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(edsDamage);
            }

            if (collider.TryGetComponent<ExplosiveBarrel>(out ExplosiveBarrel explosiveBarrel))
            {
                if (!explosiveBarrel.isExploding)
                {
                    explosiveBarrel.Hit();
                }
            }

            if (collider.TryGetComponent<EnemyProjectile>(out EnemyProjectile projectile))
            {
                projectile.Kill();
            }
        }
    }
}
