using UnityEngine;
using UnityEngine.UI;

public class PlayerPickup : MonoBehaviour
{
    public float PickUpRadius = 2f;
    public KeyCode pickupKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.F;
    public Transform itemHoldPosition;
    public Transform itemHoldPosition2;
    public Transform itemHoldPosition3;
    public Transform itemHoldPosition4;
    public Transform playerBody;
    public Transform keyHolder;

    private GameObject heldItem;
    bool isKey;
    bool isRifle;

    [Header("Audio")]
    public AudioClip pickupSound;
    public AudioClip dropSound;
    private AudioSource audioSource;

    public Button pickButton;
    public Button dropButton;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pickButton.onClick.AddListener(() => TryPickupItem());
        dropButton.onClick.AddListener(() => DropItem());
        dropButton.gameObject.SetActive (false);
    }

    void Update()
    {
        if (Input.GetKeyDown(pickupKey))
        {
            TryPickupItem();
        }

        if (Input.GetKeyDown(dropKey))
        {
            DropItem();
        }
    }

    void TryPickupItem()
    {
        if(heldItem != null)
        {
            Debug.Log("Already holding an item. Drop it to pick up another");
            return;
        }
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, PickUpRadius);

        GameObject closestItem = null;
        float closestDistance =  PickUpRadius;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("PickupItem"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestItem = hitCollider.gameObject;
                }
                isKey = true;
            }

            else if (hitCollider.CompareTag("Rifle"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestItem = hitCollider.gameObject;
                }
                isKey = false;
                isRifle = true;
            }

            else if (hitCollider.CompareTag("OtherObject"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestItem = hitCollider.gameObject;
                }
                isKey = false;
                isRifle = false;
                
            }
        }

        if (closestItem != null)
        {
            Pickup(closestItem);
        }


    }

    void Pickup(GameObject item)
    {
        heldItem = item;
        heldItem.GetComponent<Collider>().enabled = false;
        if (heldItem.GetComponent<Rigidbody>())
        {
            heldItem.GetComponent<Rigidbody>().isKinematic = true;

        }

        if(isKey)
        {
            heldItem.transform.SetParent(playerBody);
            heldItem.transform.position = itemHoldPosition.position;
            heldItem.transform.rotation = itemHoldPosition.rotation;
        }
        else if (isRifle)
        {
            heldItem.transform.SetParent(playerBody);
            heldItem.transform.position = itemHoldPosition2.position;
            heldItem.transform.rotation = itemHoldPosition2.rotation;
        }
        else
        {
            heldItem.transform.SetParent(playerBody);
            heldItem.transform.position = itemHoldPosition3.position;
            heldItem.transform.rotation = itemHoldPosition3.rotation;
        }

        Debug.Log("Picked up " + item.name);
        //sound
        PlaySound(pickupSound);
        pickButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(true);
    }

    void DropItem()
    {
        if (heldItem != null)
        {
            heldItem.GetComponent<Collider>().enabled = true;
            heldItem.transform.position = transform.position + transform.forward;
            if (heldItem.GetComponent<Rigidbody>())
            {
                heldItem.GetComponent<Rigidbody>().isKinematic= false;
            }
            heldItem.transform.SetParent(keyHolder);
            Debug.Log("Dropped " + heldItem.name);
            heldItem = null;
            //sound
            PlaySound(dropSound);
            //granny to know
            VillianAi grannyAi = FindObjectOfType<VillianAi>();
            if (grannyAi != null)
            {
                grannyAi.OnSoundHeard(transform.position);
            }


            isKey = false;
            isRifle = false;

            pickButton.gameObject.SetActive(true);
            dropButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("No item to drop.");
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
