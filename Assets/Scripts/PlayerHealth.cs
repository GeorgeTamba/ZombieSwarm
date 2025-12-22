using UnityEngine;
using UnityEngine.UI; // Required for UI
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI References")]
    public Image healthBarFill; // Drag the RED image here

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        
        // Clamp prevents health from going below 0 or above Max
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            // Convert health to a 0.0 to 1.0 percentage
            healthBarFill.fillAmount = currentHealth / maxHealth; 
        }
    }

    void Die()
    {
        // Optional: Save High Score here later
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}