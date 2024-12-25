using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isEasy;
    public bool isMobile;
    public int maxDays = 3;
    int currentDays;
    public bool allLocksOpen;
    public Text daysText;
    public GameObject cutScene;
    public static class PlayerPrefsExtension
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
    void Start()
    {
        isEasy = PlayerPrefsExtension.GetBool("isEasy", true);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        currentDays = maxDays;
        UpdateDaysText();
    }
    void Update()
    {
        if (allLocksOpen)
        {
            StartCoroutine(CutScene());
        }
    }
    public void DecreaseDay()
    {
        currentDays--;
        UpdateDaysText();
        if (currentDays <= 0)
        {
            EndGame();
        }
    }
    void EndGame()
    {
        SceneManager.LoadScene("Main Menu");
    }
    void UpdateDaysText()
    {
        if (daysText != null)
        {
            daysText.text = "Days left " + currentDays;
        }
    }
    IEnumerator CutScene()
    {
        yield return new WaitForSeconds(1);
        cutScene.SetActive(true);
        yield return new WaitForSeconds(8f);
        EndGame();
    }
}