using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody player;

    private void Awake()
    {
        player = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.velocity.magnitude < 9) // Speed limit
        {
            player.AddForce(new Vector3(Input.GetAxis("Horizontal") * 100, 0, Input.GetAxis("Vertical") * 100)); // Player movement
        }
    }
}
