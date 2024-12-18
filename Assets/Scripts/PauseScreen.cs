using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    private string currentMenu = "pause";

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(currentMenu == "options") {
                BackButton();
            } else {
                ResumeButton();
            }
        }
    }

    public void ResumeButton() 
    {
        Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void OptionsButton()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        currentMenu = "options";
    }

    public void BackButton() {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        currentMenu = "pause";
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void QuitToDesktopButton()
    {
        Application.Quit();
    }
}
