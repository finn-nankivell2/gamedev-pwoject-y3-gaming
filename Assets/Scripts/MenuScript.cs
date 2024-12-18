using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;
    public GameObject levelSelect;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
        MainMenuButton();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            MainMenuButton();
        }
    }

    public void PlayNowButton()
    {
        // load the level select menu
        mainMenu.SetActive(false);
        levelSelect.SetActive(true);
    }

    public void OptionsMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void CreditsButton()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void MainMenuButton()
    {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        levelSelect.SetActive(false);
        optionsMenu.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}