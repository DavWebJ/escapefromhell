using BlackPearl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(AudioSource))]
public class AIManager : MonoBehaviour
{
    public bool autoWandering;
    public bool iswandering;
    public bool isPerformingAction;
    public bool isInterracting;
    public bool isRotatingWithRootMotion;
    public bool canRotate;
    AiLocomotionManager aiLocomotion;
    AiAnimatorManager aiAanimator;
    public State currentState;

    [Header("ref:")]
    private AudioSource AudioSource;
    private AudioSource swordAudio;
    public EnemyObject enemy;
    public Rigidbody rb;
    public Transform UiTransform;
    public AIDamageDealer damageDealer;
    public NavMeshAgent agent;
    

    [Header("wandering parameters")]
    [SerializeField] private float wanderingWaitTimeMin;
    [SerializeField] private float wanderingWaitTimeMax;
    [SerializeField] private float wanderingDistanceMin;
    [SerializeField] private float wanderingDistanceMax;


    [Header("Movement")]
    public float rundistance = 5;
    public float walkDistance = 2.5f;
    public float rotationSpeed = 25;
    public float walkspeed;
    public float runspeed;
    public float currentSpeed;
    public float transitionSpeed = 25;



    [Header("Stat AI")]
    public bool isAttacking;
    private int index;
    public VitalState vital = null;
    public float maxHealth = 100;
    public float currentHealth;
    public bool isDead = false;
    public bool canAttack;
    public AudioClip[] footStep;
    public HealthUi healthUi;

    [Header("Detection:")]
    public float detectionRadius = 20;
    public LayerMask detectionMask;
    public LayerMask ObstacleMask;
    public LayerMask layersToIgnoreForLineOfSight;
    public float minAngle = -50;
    public float maxAngle = 50;
    public VitalState currentTarget;

    [Header("Combat")]
    public float attackCoolDown = 2f;
    public float maximumAttackRange = 1.5f;
    public float ChaseRange = 4f;
    public float timePassed;
    public float newDestinationCoolDown = 0.2f;
    public float currentRecoveryTime = 0;
    public float comboLikelyHood = 75;
    public bool canPary;
    public float chanceToPary = 50;

    public float wait;
    private void Awake()
    {
        currentHealth = maxHealth;
    }
    void Start()
    {
    

        GameObject uiGO = Instantiate(enemy.UiObject, UiTransform);
   
        agent = GetComponentInChildren<NavMeshAgent>();
        AudioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        swordAudio = transform.Find("swordAudio").GetComponent<AudioSource>();
        AudioSource.loop = false;
        AudioSource.playOnAwake = false;
        AudioSource.volume = 1;
        healthUi = uiGO.GetComponentInChildren<HealthUi>();
        healthUi.SetHealth(currentHealth, maxHealth);

        aiAanimator = GetComponent<AiAnimatorManager>();
        agent.enabled = false;
        rb.isKinematic = false;
        walkspeed = enemy.wanderSpeed;
        runspeed = enemy.chaseSpeed;

        UiTransform.GetComponentInChildren<Canvas>().gameObject.SetActive(true);


        //healthUi.ToggleUI();
    }


    public void HandleStateMachine()
    {
        if (isDead)
            return;

        if(vital != null)
        {
            if (vital.isdead)
            {
                agent.enabled = false;
                aiAanimator.animator.SetFloat("Vertical", 0);
                rb.velocity = Vector3.zero;
                return;
            }
                
        }

        if(currentState != null)
        {
           
            State nextState = currentState.Tick(this, aiAanimator);

            if(nextState != null)
            {
         
                SwitchToNextState(nextState);
            }
        }
    }

    public void SwitchToNextState(State state)
    {
        currentState = state;
    }

    private void HandleRecoveryTime()
    {
        if(currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isPerformingAction)
        {
            if(currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }
    }

    void Update()
    {
        HandleRecoveryTime();
        HandleStateMachine();
        isRotatingWithRootMotion = aiAanimator.animator.GetBool("isRotateWithRootMotion");
        isPerformingAction = aiAanimator.animator.GetBool("isInterracting");
        canRotate = aiAanimator.animator.GetBool("canRotate");
    }

    private void FixedUpdate()
    {
            
    }

    public void DisableCollider()
    {
        GetComponent<CapsuleCollider>().enabled = false;
    }

    public void EnableCollider()
    {
        GetComponent<CapsuleCollider>().enabled = true;
    }

    private void LateUpdate()
    {
        agent.transform.localPosition = Vector3.zero;
        agent.transform.localRotation = Quaternion.identity;
    }

    public void PlaySwordActionInputSound()
    {
        //if (!swordAudio.isPlaying)
        //{
        //    swordAudio.PlayOneShot(AudioM.instance.sword);
        //}

    }
    public void FootR()
    {
       
        int i = Random.Range(0, footStep.Length);

        AudioSource.clip = footStep[i];
        if(!AudioSource.isPlaying)
            AudioSource.Play();

        return;
    }
    public void FootL()
    {
        
        int i = Random.Range(0, footStep.Length);

        AudioSource.clip = footStep[i];
        if (!AudioSource.isPlaying)
            AudioSource.Play();

        return;
    }

    public void TakeDamage(float damage)
    {

        currentHealth -= damage;
        aiAanimator.PlayTargetAnimation("Hit", true);
        canPary = true;
        if (currentHealth <= 0)
        {
            StartCoroutine(PlayDeath());
        }


        healthUi.SetHealth(currentHealth, maxHealth);

    }

    public IEnumerator PlayDeath()
    {

        isDead = true;
        aiAanimator.PlayTargetAnimation("Die",true);
        
        // spawn pickup object
        yield return new WaitForSeconds(20f);
        enabled = false;

        Destroy(gameObject);
    }





    public void StartDealDamage()
    {
        isPerformingAction = true;
        damageDealer.StartDealDamage();
    }

    public void EndDealDamage()
    {
        isPerformingAction = false;
        damageDealer.EndDealDamage();
        
    }


    public void StopPlayerPerformingAction()
    {
        isPerformingAction = false;
        isInterracting = false;
        aiAanimator.animator.SetBool("isInterracting", false);
        aiAanimator.animator.applyRootMotion = false;
    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maximumAttackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);
    }
}
