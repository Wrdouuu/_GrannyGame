using UnityEngine;
using UnityEngine.UI;

public class ShootingController : MonoBehaviour
{
    public GameObject player;
    public Camera playerCamera;
    public float shootRange = 100f;
    public int maxBullets = 3;
    public float shootCooldown = 0.5f;
    public float giveDamageOf = 40f;
    public AudioClip shootingSound;
    public AudioSource audioSource;
   

    private int currentBullets;
    private float lastShootTime;

    public Button shootButton;


    void Start()
    {
        currentBullets = maxBullets;

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > lastShootTime + shootCooldown && isHoldingGun()) //&& GameManager.instance.isMobile == false)
        {
            Shoot();
        }
        //if (shootButton != null)
        //{
            //shootButton.onClick.AddListener(OnShootButtonPressed);
        //}
    }

    //void OnShootButtonPressed()
   // {
        //if (Time.time > lastShootTime + shootCooldown && isHoldingGun() && GameManager.instance.isMobile)
        //{
           // Shoot();
        //}
    //}

    bool isHoldingGun()

    {
        foreach (Transform child in player.transform)
        {
            if (child.gameObject.layer == LayerMask.NameToLayer("Rifle"))
            {
                return true;
            }
        }
        return false;
    }

    void Shoot()
    {
        if (currentBullets > 0)
        {
            lastShootTime = Time.time;
            currentBullets--;

            if (shootingSound != null)
            {
                audioSource.PlayOneShot(shootingSound);
            }

            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, shootRange))
            {
                Debug.Log("Hit: " + hit.collider.name);

                //Damage Granny
                VillianAi villianAI = hit.transform.GetComponent<VillianAi>();
                if (villianAI != null)
                {
                    villianAI.CharacterHitDamage(giveDamageOf);
                }
            }

            Debug.Log("Shot fired, bullets left: " + currentBullets);

        }
        else 
        {
            Debug.Log("No bullet left");
        }

    }
}
