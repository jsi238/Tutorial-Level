using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Player : MonoBehaviour
{
    private GameObject player;
    private Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerPos = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(playerPos.position);
        Debug.Log(this.transform.position);
        Vector3 newPos = new Vector3(playerPos.position.x, playerPos.position.y, this.transform.position.z);
        this.gameObject.transform.position = newPos;
    }
}
