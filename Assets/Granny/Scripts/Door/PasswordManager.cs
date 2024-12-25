using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordManager : MonoBehaviour
{
    public Button button0;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;
    public Button button7;
    public Button button8;
    public Button button9;
    public Button goButton;
    public Button noButton;

    public GameObject lockPanel;
    public GameObject lockGameObject;
    public Light pointLight;
    public float activationDistance = 1.6f;

    private string correctPassword = "0741";
    private string enteredPassword = "";
    private int guessCount = 0;
    private int maxGuesses = 3;
    private bool doorUnlocked = false;
    private Transform playerTransform;

    public AudioClip buttonPressSound;
    public AudioClip incorrectPasswordSound;
    public AudioClip doorUnlockedSound;
    AudioSource audioSource;

    public Button activateLockButton;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        button0.onClick.AddListener(() => OnDigitButtonClicked("0"));
        button1.onClick.AddListener(() => OnDigitButtonClicked("1"));
        button2.onClick.AddListener(() => OnDigitButtonClicked("2"));
        button3.onClick.AddListener(() => OnDigitButtonClicked("3"));
        button4.onClick.AddListener(() => OnDigitButtonClicked("4"));
        button5.onClick.AddListener(() => OnDigitButtonClicked("5"));
        button6.onClick.AddListener(() => OnDigitButtonClicked("6"));
        button7.onClick.AddListener(() => OnDigitButtonClicked("7"));
        button8.onClick.AddListener(() => OnDigitButtonClicked("8"));
        button9.onClick.AddListener(() => OnDigitButtonClicked("9"));

        goButton.onClick.AddListener(OnGoButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);

        activateLockButton.onClick.AddListener(OnActivateLockButtonClicked);

        lockPanel.SetActive(false);

        audioSource = playerTransform.gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
       if( Vector3.Distance(playerTransform.position, lockGameObject.transform.position) > activationDistance)
        {
            lockPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnActivateLockButtonClicked();
        }
    }

    void OnActivateLockButtonClicked()
    {
        if (Vector3.Distance(playerTransform.position, lockGameObject.transform.position) <= activationDistance)
        {
            lockPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void OnDigitButtonClicked(string digit)
    {
        if (enteredPassword.Length < 4 && guessCount < maxGuesses)
        {
            enteredPassword += digit;
            //sound
            PlaySound(buttonPressSound);
        }
    }

    void OnGoButtonClicked()
    {
        if (guessCount < maxGuesses)
        {
            if(enteredPassword == correctPassword)
            {
                doorUnlocked = true;
                Debug.Log("Door Unlocked");
                lockPanel.SetActive(false);
                pointLight.color = Color.green;
                //sound
                PlaySound(doorUnlockedSound);
            }
            else
            {
                guessCount++;
                Debug.Log("Incorrect password. Attempt " + guessCount + "of" + maxGuesses);
                //sound
                PlaySound(incorrectPasswordSound);
            }
            enteredPassword = "";
            
        }
        if(guessCount >= maxGuesses)
        {
            Debug.Log("Maximum guesses reached");
            SetButtonInteractable(false);
        }
    }

    void OnNoButtonClicked()
    {
        enteredPassword = "";
        //sound
    }

    void SetButtonInteractable(bool interactable)
    {
        button0.interactable = interactable;
        button1.interactable = interactable;
        button2.interactable = interactable;     
        button3.interactable = interactable;
        button4.interactable = interactable;
        button5.interactable = interactable;
        button6.interactable = interactable;
        button7.interactable = interactable;
        button8.interactable = interactable;
        button9.interactable = interactable;
        noButton.interactable = interactable;
        goButton.interactable = interactable;
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
