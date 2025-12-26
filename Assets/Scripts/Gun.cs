using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public GameObject bulletPrefab;

    [Header("Settings")]
    public float fireRate = 0.5f; 
    private float nextFireTime = 0f;

    public void TryShoot()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        // 1. Calculate the Rotation
        // Instead of using 'transform.rotation' (which wobbles with the hand animation),
        // we use the Player's Root rotation. This ensures bullets always fly straight forward.
        
        // This grabs the rotation of the topmost parent (The Player Character)
        Quaternion stableRotation = transform.root.rotation; 

        // 2. Instantiate the Bullet
        // Position: Gun Barrel (So it looks real)
        // Rotation: Player Body (So it shoots straight)
        Instantiate(bulletPrefab, firePoint.position, stableRotation);
    }
}