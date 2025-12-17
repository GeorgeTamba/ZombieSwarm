using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public GameObject bulletPrefab;

    [Header("Settings")]
    public float fireRate = 0.5f; // Time between shots
    private float nextFireTime = 0f;

    public void TryShoot()
    {
        // Check if enough time has passed to fire again
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, transform.rotation);
    }
}