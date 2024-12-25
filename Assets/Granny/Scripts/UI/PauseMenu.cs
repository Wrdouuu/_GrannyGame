using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button resumeButton;
    public Button pauseButton;
    public Button mainMenuButton;

    private bool isPaused = false;

    public GameObject mobileControlsUI;

    void Start()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        pauseButton.onClick.AddListener(PauseGame);
        mainMenuButton.onClick.AddListener(BackToMainMenu);

        pauseMenuUI.SetActive(false);
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            mobileControlsUI.SetActive(false);
            isPaused = true;
        }
    }

    public void ResumeGame()
    {
        if (isPaused)
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            mobileControlsUI.SetActive(true);
            isPaused = false;
        }
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
