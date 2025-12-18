using UnityEngine;
using UnityEngine.SceneManagement; // Needed to restart the game

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"Player Hit! Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("GAME OVER");
        // Simple "Game Over": Reload the current scene to restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}