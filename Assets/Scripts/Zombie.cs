using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [Header("Stats")]
    public float health = 3f;
    public float damage = 10f;
    public float attackSpeed = 1.5f; 
    private float nextAttackTime = 0f;

    [Header("References")]
    public Animator animator;      
    public Collider bodyCollider;  
    private Transform player;
    private NavMeshAgent agent;
    private bool isDead = false;

    [Header("Animation Sync")]
    public float baseSpeed = 0.3f; // The speed where the walk animation looks correct

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (isDead) return;

        if (player != null)
        {
            // 1. CHASE PLAYER
            agent.SetDestination(player.position);

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            float attackRange = 1.5f; 

            // 2. DECIDE: ATTACK OR MOVE?
            if (distanceToPlayer <= attackRange)
            {
                // --- ATTACK MODE ---
                
                // Stop moving visually
                if (animator != null) 
                {
                    animator.SetFloat("Speed", 0f);
                    animator.SetFloat("WalkMultiplier", 1f);
                }

                // Check Cooldown
                if (Time.time >= nextAttackTime)
                {
                    // Trigger the animation. 
                    // The damage will be handled by the Animation Event later.
                    if (animator != null) animator.SetTrigger("Attack");
                    
                    nextAttackTime = Time.time + attackSpeed; 
                }
            }
            else
            {
                // --- MOVEMENT MODE ---
                if (animator != null)
                {
                    float currentSpeed = agent.velocity.magnitude;
                    
                    // Sync "Idle vs Walk" Blend Tree
                    animator.SetFloat("Speed", currentSpeed);
                    
                    // Sync Foot Sliding
                    float multiplier = Mathf.Max(1f, currentSpeed / baseSpeed);
                    animator.SetFloat("WalkMultiplier", multiplier);
                }
            }
        }
    }

    // ---------------------------------------------------------
    // PUBLIC EVENTS
    // ---------------------------------------------------------

    // CALLED BY: ZombieAnimationRelay.cs (Triggered by Animation Event)
    public void DealDamageEvent()
    {
        // Check if player is still in range to avoid "phantom hits"
        if (player != null && Vector3.Distance(transform.position, player.position) <= 2.0f)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    // CALLED BY: Bullet.cs (or whatever deals damage to zombies)
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;

        if (health <= 0) Die();
    }

    void Die()
    {
        isDead = true;

        if (animator != null) animator.SetTrigger("Die");

        // Disable Physics & AI
        agent.isStopped = true;       
        agent.enabled = false;        
        bodyCollider.enabled = false; 

        Destroy(gameObject, 5f);
    }
}