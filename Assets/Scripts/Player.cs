using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Character Properties")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float speed = 1;
    [SerializeField] private bool hasHitExplosion;

    [Header("Gun Properties")]
    [SerializeField] private int gunDamage = 1;
    [SerializeField] private int pelletCount = 10;
    [SerializeField] private float reloadTime = 0.5f;
    
    private int health;

    [HideInInspector] public bool canControl = true;

    private bool upPressed;
    private bool downPressed;
    private bool rightPressed;
    private bool leftPressed;
    private bool firePressed;
    private bool reloadPressed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

                if (Input.GetKey(KeyCode.Mouse1))
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
}
