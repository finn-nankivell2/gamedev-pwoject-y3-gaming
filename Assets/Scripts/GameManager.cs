using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEditor.Purchasing;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public Transform respawnPosition;
    public GameObject sceneTransition;
    public CinemachineFreeLook freeLookCamera;
    public GameObject player;

    [System.NonSerialized]
    public PlayerMovementFreecam playerScript;

    public GameObject canvas;

    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        animator = sceneTransition.GetComponent<Animator>();
        playerScript = player.GetComponent<PlayerMovementFreecam>();
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpawnPosition(Transform newPosition)
    {
        respawnPosition.position = newPosition.position;
        respawnPosition.rotation = newPosition.rotation;
    }

    public void RespawnPlayer(Collider player) {
        StartCoroutine(RespawnPlayerCoroutine(player));
    }

    public void LoadNextLevel() {
        Debug.Log("time to load the next scene!");
    }

    public IEnumerator RespawnPlayerCoroutine(Collider player)
    {
        animator.Play("SlideIn");
        
        yield return new WaitForSeconds(0.8f);

        PlayerMovementFreecam playerMovement = player.GetComponent<PlayerMovementFreecam>();
        playerMovement.ResetSpeedMod();

        player.transform.position = respawnPosition.position;
        player.transform.rotation = respawnPosition.rotation;
        freeLookCamera.m_XAxis.Value = respawnPosition.rotation.y * (360/Mathf.PI);
        Physics.SyncTransforms();

        animator.Play("SlideOut");
    }
}