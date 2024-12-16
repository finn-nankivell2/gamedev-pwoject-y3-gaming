using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    public Transform spawnPos;

    void Start()
    {
        // newSpawnPosition = GetComponentInChild()
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.Instance.SetSpawnPosition(spawnPos);
            PlayerMovementFreecam playerMovement = other.GetComponent<PlayerMovementFreecam>();
            playerMovement.AddJump();
            Destroy(gameObject);
            GameManager.Instance.particleManager.Play("battery", transform.position);
            GameManager.Instance.audioManager.Play("battery", transform.position);
        }
        
    }
}
