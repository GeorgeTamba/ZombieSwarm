using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [Header("Stats")]
    public float health = 3f;
    public float damage = 10f;       // How hard the zombie hits
    public float attackSpeed = 1f;   // Time between attacks (seconds)

    private float nextAttackTime = 0f;

    [Header("References")]
    private Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    // --- NEW: COLLISION ATTACK LOGIC ---
    void OnCollisionStay(Collision collision)
    {
        // Check if we are touching the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if enough time has passed to attack again
            if (Time.time >= nextAttackTime)
            {
                Attack(collision.gameObject);
                nextAttackTime = Time.time + attackSpeed;
            }
        }
    }

    void Attack(GameObject target)
    {
        // Get the PlayerHealth script from the object we hit
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }
    // -----------------------------------

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0) Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}