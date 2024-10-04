using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [Header("Gun Properties")]
    [SerializeField] private int gunDamage = 1;
    [SerializeField] private float shotCooldown = 0.3f;
    [SerializeField] private int pelletCount = 10;
    [SerializeField] private float reloadTime = 0.5f;
    [SerializeField] private int clipSize = 3;

    [Header("References")]
    [SerializeField] private GameObject player;

    private int bulletsInClip;

    private Vector2 cursorDirection;

    // Start is called before the first frame update
    void Start()
    {
        bulletsInClip = clipSize;
    }

    // Update is called once per frame
    void Update()
    {
        cursorDirection = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.GetComponent<Rigidbody2D>().position).normalized;

        transform.position = player.GetComponent<Rigidbody2D>().position + (cursorDirection * 2);

        float angle = Mathf.Atan2(cursorDirection.y, cursorDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Fire()
    {
        if (bulletsInClip == 0)
            return;


    }

    public void Reload()
    {
        if (bulletsInClip == clipSize)
            return;

        bulletsInClip++;
    }
}
