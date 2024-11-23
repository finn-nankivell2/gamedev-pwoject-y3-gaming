using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerMovement2 playerMovement = other.GetComponent<PlayerMovement2>();
            playerMovement.AddJump();
            Destroy(gameObject);
        }
        
    }
}
