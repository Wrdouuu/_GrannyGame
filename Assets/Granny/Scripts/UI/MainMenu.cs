using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public static class PlayerPrefsExtensions
{
    public static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }
    public static bool GetBool(string key, bool defaultValue)
    {
        return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
    }
}
public class MainMenu : MonoBehaviour
{
    public GameObject buttonPanels;
    public GameObject optionsPanel;
    public Button playButton;
    public Button optionsButton;
    public Button backButton;
    public Button easyButton;
    public Button hardButton;
    void Start()
    {
        buttonPanels.SetActive(true);
        optionsPanel.SetActive(false);
        playButton.onClick.AddListener(PlayGame);
        optionsButton.onClick.AddListener(ShowOptions);
        backButton.onClick.AddListener(ShowMainMenu);
        easyButton.onClick.AddListener(SetEasyDifficulty);
        hardButton.onClick.AddListener(SetHardDifficulty);
        // Sử dụng PlayerPrefsExtensions để gọi GetBool
        if (PlayerPrefsExtensions.GetBool("isEasy", true))
        {
            easyButton.gameObject.SetActive(false);
            hardButton.gameObject.SetActive(true);
        }
        else
        {
            easyButton.gameObject.SetActive(true);
            hardButton.gameObject.SetActive(false);
        }
    }
    void PlayGame()
    {
        SceneManager.LoadScene("Mission");
    }
    void ShowOptions()
    {
        buttonPanels.SetActive(false);
        optionsPanel.SetActive(true);
    }
    void ShowMainMenu()
    {
        buttonPanels.SetActive(true);
        optionsPanel.SetActive(false);
    }
    void SetEasyDifficulty()
    {
        PlayerPrefsExtensions.SetBool("isEasy", true);
        PlayerPrefs.Save();
        easyButton.gameObject.SetActive(false);
        hardButton.gameObject.SetActive(true);
    }
    void SetHardDifficulty()
    {
        PlayerPrefsExtensions.SetBool("isEasy", false);
        PlayerPrefs.Save();
        easyButton.gameObject.SetActive(true);
        hardButton.gameObject.SetActive(false);
    }
}