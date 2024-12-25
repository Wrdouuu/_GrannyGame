using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class VillianAi : MonoBehaviour
{
    [Header("Health System")]

    float characterHealth = 100f;
    public float presentHealth;
    public float respawnTime = 5f;

    public GameObject grannyDeathText;
    public Text deathText;

    [Header ("Sound Detection")]
    NavMeshAgent navMeshAgent;
    public GameObject grannyDeadBody;

    public float moveSpeed = 3.5f;
    public Transform startPosition;

    private Vector3 soundLocation;
    public bool isReturning = false;
    private bool isChasing = false;
    private bool isAttacking = false;
    private bool isWaiting = false;
    private bool isDead = false;
    bool soundHeard = false;

    [Header("Granny States")]
    public Transform player;
    public float detectionRadius = 7f;
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    float lastAttackTime = 0f;

    private Animator animator;

    [Header("Footstep")]
    public AudioClip[] footstepSounds;

    AudioSource audioSource;
    float footstepInterval = .5f;
    float nextFootstepTime = 0f;


    void Start()
    {
        presentHealth = characterHealth;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        grannyDeathText.SetActive(false);
    }

    void Update()
    {
        if (isDead) return;
        if (isReturning)
        {
            ReturnToStart();
        }
        else if(isAttacking)
        {
            AttackPLayer();
        }
        else if(isChasing && soundHeard)
        {
            ChasePlayer();
        }
        else if (!isWaiting)
        {
            LookForPlayer();
        }
        UpdateAnimation();
        PlayFootstepSound();



        if(GameManager.instance.isEasy)
        {
            deathText.text = "Granny is gone for 2 mins";
            respawnTime = 120f;
        }
        else
        {
            deathText.text = "Granny is gone for 1 min";
            respawnTime = 60f;
        }

    }

    public void OnSoundHeard(Vector3 location)
    {
        if (isDead) return;

        soundLocation = location;
        soundHeard = true;
        isReturning = false;
        isAttacking = false;
        isChasing = false;
        isWaiting = false;
        MoveToSoundLocation();

    }

    void MoveToSoundLocation()
    {
        navMeshAgent.SetDestination(soundLocation);
        if (Vector3.Distance(transform.position, soundLocation) <= navMeshAgent.stoppingDistance)
        {
            StartCoroutine(WaitBeforeReturning());
        }
    }
    IEnumerator WaitBeforeReturning()
    {
        isWaiting = true;
        yield return new WaitForSeconds(10f);
        isWaiting = false;
        isReturning = true;
        soundHeard = false;
    }

    void ReturnToStart()
    {
        navMeshAgent.SetDestination(startPosition.position);
        if (Vector3.Distance(transform.position, startPosition.position) <= navMeshAgent.stoppingDistance)
        {
            isReturning = false;
        }

    }

    void LookForPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.CompareTag("Player"))
            {
                isChasing = true;
                isReturning = false;
                break;
            }
        }
    }

    void ChasePlayer()
    {
        navMeshAgent.SetDestination(player.position);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            navMeshAgent.isStopped = true;
            isChasing = false;
            isAttacking = true;
        }
        else if (distanceToPlayer > detectionRadius)
        {
            isChasing = false;
            isReturning = true;
        }
    }

    void AttackPLayer()
    {
        if(Time.time > lastAttackTime + attackCooldown)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                //Damage the player
                Debug.Log("AttackPlayer");
                
                playerController.TakeDamage(100f);
                
            }
            lastAttackTime = Time.time;
            //Respawn Granny at Start Position
            StartCoroutine(Respawn(2));

            float distanceToPlayer = Vector3.Distance (transform.position, player.position);
            if(distanceToPlayer > attackRange)
            {
                navMeshAgent.isStopped = false;
                isChasing = true;
                isAttacking = false;
            }
        }
    }

    void UpdateAnimation()
    {
        bool isMoving = navMeshAgent.velocity.magnitude > 0.1f;
        animator.SetBool("isWalking",isMoving);
        animator.SetBool("isAttacking",isAttacking);
        animator.SetBool("isDead", isDead);

        if (!isMoving && !isAttacking && !isDead)
        {
            animator.SetBool("isIdle", true);

        }
        else
        {
            animator.SetBool("isIdle", false);

        }
    }

    void PlayFootstepSound()
    {
        if(navMeshAgent.velocity.magnitude > 0.1f && Time.time >= nextFootstepTime )
        {
            if(footstepSounds.Length > 0)
            {
                AudioClip footstepSound = footstepSounds[Random.Range(0, footstepSounds.Length)];
                audioSource.PlayOneShot(footstepSound);
                nextFootstepTime = Time.time + footstepInterval;
            }
        }
    }

    public void CharacterHitDamage(float takeDamage)
    {
        if (isDead) return;

        presentHealth -= takeDamage;
        if(presentHealth <= 0)
        {
            characterDie();
        }

    }
    
    void characterDie()
    {
        isDead = true;
        moveSpeed = 0;
        navMeshAgent.speed = moveSpeed;
        detectionRadius = 0;

        animator.SetBool("isDead", true);
        GetComponent<Collider>().enabled = false;

        navMeshAgent.enabled = false;

        //UI
        grannyDeathText.SetActive(true);
        

        //Respawn Granny
        StartCoroutine(Respawn(respawnTime));
    }

    IEnumerator Respawn(float Delay)
    {
        yield return new WaitForSeconds(3f);
        grannyDeathText.SetActive(false);
        grannyDeadBody.SetActive(false);
        yield return new WaitForSeconds(Delay - 3);

        presentHealth = characterHealth;
        isDead = false;
        animator.SetBool("isDead", false);

        GetComponent<Collider>().enabled = true;
        navMeshAgent.enabled=true;
        this.enabled = true;

        transform.position = startPosition.position;
        navMeshAgent.Warp(startPosition.position);

        isReturning = false;
        isChasing = false;
        isAttacking = false;
        isWaiting = false;
        soundHeard = false;
        moveSpeed = 3.5f;
        navMeshAgent.speed = moveSpeed;
        detectionRadius = 15;
    }
}
