using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [Header("Stats")]
    public float health = 3f;
    public float damage = 10f;
    public float attackSpeed = 1.5f; // Slower than player shooting
    private float nextAttackTime = 0f;

    [Header("References")]
    public Animator animator;      // Drag the Animator here
    public Collider bodyCollider;  // Drag the Capsule Collider here
    private Transform player;
    private NavMeshAgent agent;
    private bool isDead = false;

    [Header("Animation Sync")]
    public float baseSpeed = 0.3f; // SET THIS to the speed that looked "perfect" before!

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // Find player automatically
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (isDead) return;

        // 1. CHASE PLAYER
        if (player != null)
        {
            agent.SetDestination(player.position);
        }

        // 2. ANIMATE MOVEMENT
        if (animator != null)
        {
            float currentSpeed = agent.velocity.magnitude;

            // A. The Blend Tree parameter (Switching from Idle to Walk)
            animator.SetFloat("Speed", currentSpeed);

            // B. The Multiplier (Speeding up the Walk animation)
            // We use Mathf.Max(1, ...) to ensure Idle animation doesn't freeze or slow down
            float multiplier = Mathf.Max(1f, currentSpeed / baseSpeed);
            animator.SetFloat("WalkMultiplier", multiplier);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (isDead) return;

        // Attack Logic
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= nextAttackTime)
            {
                Attack(collision.gameObject);
                nextAttackTime = Time.time + attackSpeed;
            }
        }
    }

    void Attack(GameObject target)
    {
        // Trigger Animation
        if (animator != null) animator.SetTrigger("Attack");

        // Deal Damage
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;
        
        // Optional: Add a small "Flinch" animation here later?

        if (health <= 0) Die();
    }

    void Die()
    {
        isDead = true;

        // 1. Play Animation
        if (animator != null) animator.SetTrigger("Die");

        // 2. Disable Physics & Movement
        agent.isStopped = true;       // Stop moving
        agent.enabled = false;        // Turn off AI
        bodyCollider.enabled = false; // Stop blocking bullets/player

        // 3. Destroy body after 5 seconds (so we can see the dead body for a bit)
        Destroy(gameObject, 5f);
    }
}