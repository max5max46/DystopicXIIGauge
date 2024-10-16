using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Properties")]
    public int maxHealth = 3;
    [SerializeField] private float immunityTime = 0.5f;
    [SerializeField] private float maxSpeed = 10;
    [SerializeField] private float acceleration = 10;
    [SerializeField] private float decelerationMultiplier = 0.8f;

    [Header("References")]
    [SerializeField] private Shotgun shotgun;
    [SerializeField] private UIManager uiManager;

    [HideInInspector] public int health;
    private Rigidbody2D rb;
    private Vector2 movementVector;

    [HideInInspector] public int parts;
    [HideInInspector] public bool canControl;

    private bool upPressed;
    private bool downPressed;
    private bool rightPressed;
    private bool leftPressed;
    private bool firePressed;
    private bool reloadPressed;

    // Start is called before the first frame update
    void Start()
    {
        parts = 0;
        canControl = true;
        health = maxHealth;

        rb = GetComponent<Rigidbody2D>();

        ManageInputs(true);
    }

    // Update is called once per frame
    void Update()
    {
        ManageInputs();

        SetMovementVector();

        if (firePressed)
            shotgun.Fire();

        if (reloadPressed)
            shotgun.Reload();

        ManageInputs(true);
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < maxSpeed)
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

                if (Input.GetKeyDown(KeyCode.Mouse0))
                    firePressed = true;

                if (Input.GetKeyDown(KeyCode.Mouse1))
                    reloadPressed = true;
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

        movementVector = new Vector2(moveLeftRight, moveForwardBackward).normalized * acceleration;
    }

    public void TakeDamage(int damage)
    {
        if (health < 1)
            return;

        health -= damage;

        if (health < 1)
            GameOver();
    }

    private void GameOver()
    {
        canControl = false;
        uiManager.SwitchUIScreen("results");
    }


    public void ReceiveParts(int partsToReceive)
    {
        parts += partsToReceive;
    }
}
