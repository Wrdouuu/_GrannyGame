using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    Animator doorAnimator;
    public Transform player;
    public float detectionDistance = 3f;
    public LayerMask playerLayer;
    private bool isPlayerNear = false;
    public string keyLayerName = "";

    public AudioClip doorOpenSound;
    AudioSource audioSource;

    void Start()
    {
        doorAnimator = GetComponent<Animator>();
        audioSource = player.gameObject.GetComponent<AudioSource>();

        GameObject mobileControls = GameObject.Find("MobileControls");
        if(mobileControls != null )
        {
            Button doorButton = mobileControls.transform.Find("DoorOpen").GetComponent<Button>();
            if (doorButton != null)
            {
                doorButton.onClick.AddListener(OnDoorButtonPress);
            }
            else
            {
                Debug.Log("Button not found");
            }
        }
        else
        {
            Debug.Log("MobileControls Gameobject not found");
        }
    }

    void Update()
    {
        checkPlayerDistance();
        if( isPlayerNear && Input.GetKeyDown(KeyCode.E) )
        {
            OnDoorButtonPress();
        }
    }
    void checkPlayerDistance()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = player.position - transform.position;

        

        if(Physics.Raycast(transform.position, directionToPlayer, out hit, detectionDistance, playerLayer))
        {
            if(hit.transform == player)
            {
                isPlayerNear = true;
                return;
            }

        }
        isPlayerNear = false;
    }

    void OnDoorButtonPress()
    {
        OpenDoor();
    }

    void OpenDoor()
    {
        if (doorAnimator != null)
        {
            if (!string.IsNullOrEmpty(keyLayerName))
            {
                bool playerHasKey = false;

                foreach (Transform key in player)
                {
                    if (key.gameObject.layer == LayerMask.NameToLayer(keyLayerName))
                    {
                        playerHasKey = true;
                        break;
                    }
                }
                if (!playerHasKey)
                {
                    return;
                }
            }

            doorAnimator.SetTrigger("Open");
            //sound
            PlaySound(doorOpenSound);
            //Granny know
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
