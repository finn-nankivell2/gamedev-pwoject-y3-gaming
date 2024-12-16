using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public Transform respawnPosition;
    public GameObject sceneTransition;
    public CinemachineFreeLook freeLookCamera;
    public GameObject player;
    public SpeedrunTimer speedrunTimer;
    public GameObject levelFinishText;
    public GameObject canvas;
    public ParticleManager particleManager;
    public List<string> levels;
    public AudioManager audioManager;
    public GameObject endLevelPlayer;


    [System.NonSerialized]
    public PlayerMovementFreecam playerScript;
    private Animator animator;
    private bool levelEndState = false;


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

        levelFinishText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (levelEndState && Input.GetKeyDown(KeyCode.Mouse0)) {
            HandleLoadingNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
			RestartLevelFresh();
		}
    }

    public void SetSpawnPosition(Transform newPosition)
    {
        respawnPosition.position = newPosition.position;
        respawnPosition.rotation = newPosition.rotation;
    }

    public void RespawnPlayer(Collider player) {
        StartCoroutine(RespawnPlayerCoroutine(player));
    }

    public void SetLevelEndState() {
        speedrunTimer.StopTimer();
        levelFinishText.SetActive(true);
        TMP_Text text = speedrunTimer.GetComponent<TMP_Text>();
        text.color = new Color(0, 255, 0);
        levelEndState = true;

        Instantiate(endLevelPlayer, player.transform.position, player.transform.rotation);
        particleManager.Play("endlevel", player.transform.position, Quaternion.AngleAxis(90, Vector3.left));
        player.SetActive(false);
    }

	public void RestartLevelFresh() {
        string currentLevel = SceneManager.GetActiveScene().name;
		SceneManager.LoadScene(currentLevel);
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

    private void HandleLoadingNextLevel()
    {
        string currentLevel = SceneManager.GetActiveScene().name;
        int index = levels.IndexOf(currentLevel);
        if(index != levels.Count-1) {
            SceneManager.LoadScene(levels[index+1]);
        } else{
            // this should be handled
            Debug.Log("no more levels!");
            SceneManager.LoadScene("BaseLevel");
        }
    }
}
