using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using BlackPearl;
using System;


public enum AIStats
{
    Idle,
    Chase,
    Wandering,
    Attack,
    Dead,
    Heat,
    Buff
}
public class AIManagerAnimal : MonoBehaviour
{
    [Header("ref:")]
    public Transform player;
    private NavMeshAgent agent;
    public Animator animator;
    [Header("Stat AI")]
    [SerializeField]
    private float detectionRadius;
    [SerializeField]
    private float attackRadius;
    public float walkSpeed;
    public float chaseSpeed;
    public bool isAttacking;
    private bool isWandering;
    public AIStats aIStats = AIStats.Idle;
    public float rotationSpeed;
    [Header("wandering parameters")]
    [SerializeField] private float wanderingWaitTimeMin;
    [SerializeField] private float wanderingWaitTimeMax;
    [SerializeField] private float wanderingDistanceMin;
    [SerializeField] private float wanderingDistanceMax;

    public AnimationClip[] attackAnimPossibility;
    private AnimationClip attackChoosing;
    private int index;

    private VitalState playerVital;

    public float maxHealth = 100;
    public float currentHealth;
    public float xpToWin = 0;
    public bool isDead = false;

    public float damage;

    public HealthUi healthUi;
    

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerVital = player.GetComponent<VitalState>();
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthUi = transform.Find("Canvas").GetComponentInChildren<HealthUi>();
        healthUi.SetHealth(currentHealth, maxHealth);
        healthUi.ToggleUI();
    }

    
    void Update()
    {
        if (GetDistanceFromPlayer() < detectionRadius && !playerVital.isdead && !isDead)
        {
            if (!healthUi.uiGameObject.activeInHierarchy)
            {
                healthUi.ToggleUI();
            }
                
           
            
            aIStats = AIStats.Chase;
            agent.speed = chaseSpeed;
            Quaternion rot = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
            if(!isAttacking)
            {
                if (GetDistanceFromPlayer() < attackRadius)
                {
                    
                    
                    StartCoroutine(CombatPlayer());
                }
                else
                {
                    StopCoroutine(CombatPlayer());
                    agent.SetDestination(player.position);
                }
            }
        }
        else
        {
            
            if (isDead)
            {
                
                return;
            }

          
            if (healthUi.uiGameObject.activeInHierarchy)
            {
                healthUi.ToggleUI();
            }
            
            agent.speed = walkSpeed;
            if(agent.remainingDistance < 0.75f && !isWandering)
            {

                aIStats = AIStats.Idle;
                StartCoroutine(Wandering());
            }
        }

        animator.SetFloat("speed", agent.velocity.magnitude);
        
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
        {
            
            return;
        }

        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            
            isDead = true;
            animator.SetTrigger("Death");
            StartCoroutine(PlayDeath());

            

        }
        else
        {
            animator.SetTrigger("Hit");
        }

        healthUi.SetHealth(currentHealth, maxHealth);

    }

    public IEnumerator PlayDeath()
    {
        agent.enabled = false;

        // spawn pickup object
        yield return new WaitForSeconds(20f);
        enabled = false;
     
        Destroy(gameObject);
    }

    public float GetDistanceFromPlayer()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        return dist;
    }
    public IEnumerator Wandering()
    {
        isWandering = true;
        
        yield return new WaitForSeconds(UnityEngine.Random.Range(wanderingWaitTimeMin, wanderingWaitTimeMax));
        Vector3 nextDestination = transform.position;
        nextDestination += UnityEngine.Random.Range(wanderingDistanceMin,wanderingDistanceMax) * new Vector3(UnityEngine.Random.Range(-1f, 1),0f, UnityEngine.Random.Range(-1f, 1)).normalized;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(nextDestination,out hit, wanderingDistanceMax, NavMesh.AllAreas))
        {
            aIStats = AIStats.Wandering;
            agent.SetDestination(hit.position);
        }
        isWandering = false;
        

    }

    public IEnumerator CombatPlayer()
    {
        GetRandomAttack();
        isAttacking = true;
        
        aIStats = AIStats.Attack;

        agent.isStopped = true;
        animator.SetTrigger(attackAnimPossibility[index].name);
        playerVital.TakeDamage(damage);
        yield return new WaitForSeconds(attackAnimPossibility[index].length);
        if (agent.enabled)
        {
            agent.isStopped = false;
        }
        isAttacking = false;
        aIStats = AIStats.Chase;
        yield break;
    }


    public int GetRandomAttack()
    {

        index = UnityEngine.Random.Range(0, attackAnimPossibility.Length);
        attackChoosing = attackAnimPossibility[index];
        return index;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
