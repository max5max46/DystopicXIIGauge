using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = FindFirstObjectByType<Player>().gameObject;

        player.transform.position = transform.position;
    }
}
