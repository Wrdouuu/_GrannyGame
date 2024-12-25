using System.Collections;
using UnityEngine;
using Terresquall;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player Health")]
    public float currentHealth;
    float maxHealth = 100f;

    private bool isDead = false;
    public Transform startPosition;

    public GameObject loadingScreen;

    [Header("Movement and Gravity")]

    public float speed = 5f;
    public float gravity = -9.81f;
    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("Camera")]
    public float mouseSensivetivity = 2f;
    
    [Header("Crouch")]
    public float crouchSpeed = 2.5f;
    public float crouchHeight = 0f;
    public float standHeight = 2.5f;
    public bool isCrouching = false;

    public Button crouchButton;
    void Start()
    {
        currentHealth = maxHealth;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        //if(crouchButton != null )
        //{
            //crouchButton.onClick.AddListener(HandleCrouch);
        //}
    }

    

    void Update()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        HandleCameraMovement();
        HandlePlayerMoveMent();

        velocity.y = gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.C))
        {
            HandleCrouch();
        }

    }

    void HandlePlayerMoveMent()
    {
        if(GameManager.instance.isMobile)
        //{
            //float currentSpeed = isCrouching ? crouchSpeed : speed;
           // float moveForwardBackward = VirtualJoystick.GetAxis("Vertical") * currentSpeed;
            //float moveLeftRight = VirtualJoystick.GetAxis("Horizontal") * currentSpeed;
            //Vector3 move = transform.right * moveLeftRight + transform.forward * moveForwardBackward;
            //characterController.Move(move * Time.deltaTime);
        //}
        //else
        {
            float currentSpeed = isCrouching ? crouchSpeed : speed;
            float moveForwardBackward = Input.GetAxis("Vertical") * currentSpeed;
            float moveLeftRight = Input.GetAxis("Horizontal") * currentSpeed;
            Vector3 move = transform.right * moveLeftRight + transform.forward * moveForwardBackward;
            characterController.Move(move * Time.deltaTime);
        }

        
    }

    void HandleCameraMovement()
    {
        if (GameManager.instance.isMobile)
        //{
            //float mouseX = VirtualJoystick.GetAxis("Horizontal", 1) * mouseSensivetivity;
            //float mouseY = VirtualJoystick.GetAxis("Vertical", 1) * mouseSensivetivity;
           // transform.Rotate(Vector3.up * mouseX);
           // float verticalLookRotation = Camera.main.transform.localEulerAngles.x - mouseY;
           // Camera.main.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0, 0);
       // }
        //else
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensivetivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensivetivity;
            transform.Rotate(Vector3.up * mouseX);
            float verticalLookRotation = Camera.main.transform.localEulerAngles.x - mouseY;
            Camera.main.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0, 0);
        }
            

    }

    public void HandleCrouch()
    {
        isCrouching = !isCrouching;
        characterController.height = isCrouching ? crouchHeight : standHeight * 0.5f;
        characterController.radius = isCrouching ? 0.2f : 0.3f;

    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }
    void Die()
    {
        isDead = true;
        //Decrease the days
        GameManager.instance.DecreaseDay();
        StartCoroutine(HandleRespawn());
    }
    IEnumerator HandleRespawn()
    {
        yield return new WaitForSeconds(2f);
        //black screen
        loadingScreen.SetActive(true);
        characterController.enabled = false;
        transform.position = startPosition.position;
        characterController.enabled = true;

        yield return new WaitForSeconds(3f);
        //black screen off
        loadingScreen.SetActive(false);
        currentHealth = maxHealth;
        isDead = false;
    }
}

