using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        print("Collision detected!");
        print("Collision caused by " + other.name);
        BroadcastMessage("__TakeDamage", 15f, SendMessageOptions.DontRequireReceiver);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
