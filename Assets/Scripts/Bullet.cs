using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 20f;
    public float damage = 1f; // How much hurt this bullet does
    public float lifeTime = 2f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Check if the object we hit has the "Zombie" script
             Zombie zombie = collision.gameObject.GetComponent<Zombie>();
            if(zombie != null)
            {
                zombie.TakeDamage(damage); // Deal damage
            }
            Destroy(gameObject);       // Destroy the bullet
        }
        // Optional: Destroy bullet if it hits a wall/ground (but not the player)
        else if (!collision.gameObject.CompareTag("Player")) 
        {
            Destroy(gameObject);
        }
    }
}