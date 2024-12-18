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
    public List<float> aceTimes;
    public AudioManager audioManager;
    public GameObject endLevelPlayer;
    public SphereCollider respawnCameraConfiner;
    public GameObject pauseMenu;
    public GameObject aceTimeIndicator;

    [System.NonSerialized]
    public PlayerMovementFreecam playerScript;
    private Animator animator;
    private bool levelEndState = false;
    private bool respawning;
    private Vector3 confinerLastPosition;

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

        pauseMenu.SetActive(false);
        levelFinishText.SetActive(false);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
		UnityEngine.Cursor.visible = false;
        Time.timeScale = 1;
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

        if (!levelEndState && Input.GetKeyDown(KeyCode.Escape)) {
            Pause();
        }
    }

    void LateUpdate()
    {
        if(respawning){
            respawnCameraConfiner.transform.position = new Vector3 (confinerLastPosition.x, confinerLastPosition.y, confinerLastPosition.z);
        }
        confinerLastPosition = respawnCameraConfiner.transform.position;
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

        string currentLevel = SceneManager.GetActiveScene().name;
        int index = levels.IndexOf(currentLevel);
        float aceTime = aceTimes[index];
        TMP_Text text = speedrunTimer.GetComponent<TMP_Text>();
        float time = speedrunTimer.GetTime();
        if(time < aceTime) {
            text.color = new Color(1, 100f/255, 1);
            aceTimeIndicator.SetActive(true);
        } else {
            text.color = new Color(0, 1, 0);
        }

        string savedTimeKey = "Level" + (index+1) + "Time";
        float savedTime = PlayerPrefs.GetFloat(savedTimeKey, 999999);
        if(time < savedTime) {
            PlayerPrefs.SetFloat(savedTimeKey, time);
        }


        levelEndState = true;


		audioManager.PlaySpatial("levelend", player.transform.position);

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
        respawning = true;
        animator.Play("SlideIn");
        audioManager.Play("death");

        yield return new WaitForSeconds(0.8f);

        PlayerMovementFreecam playerMovement = player.GetComponent<PlayerMovementFreecam>();
        playerMovement.ResetSpeedMod();

        player.transform.position = respawnPosition.position;
        player.transform.rotation = respawnPosition.rotation;
        freeLookCamera.m_XAxis.Value = respawnPosition.rotation.y * (360/Mathf.PI);
        Physics.SyncTransforms();

        animator.Play("SlideOut");
        respawning = false;
        respawnCameraConfiner.transform.localPosition = Vector3.zero;
    }

    private void HandleLoadingNextLevel()
    {
        string currentLevel = SceneManager.GetActiveScene().name;
        int index = levels.IndexOf(currentLevel);
        if(index != levels.Count-1) {
            SceneManager.LoadScene(levels[index+1]);
        } else{
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void Pause() 
    {
        pauseMenu.SetActive(true);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
		UnityEngine.Cursor.visible = true;
        Time.timeScale = 0;
    }
}
