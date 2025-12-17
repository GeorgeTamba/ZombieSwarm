using UnityEngine;
using UnityEngine.AI; // Required for NavMeshAgent

public class Zombie : MonoBehaviour
{
    [Header("Stats")]
    public float health = 3f;

    [Header("References")]
    private Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // Automatically find the player so we don't have to drag references manually
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        // Chase the player
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    // Public function for other scripts to call
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Logic for score or dropping items goes here later
        Destroy(gameObject);
    }
}