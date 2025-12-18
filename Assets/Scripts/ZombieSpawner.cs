using UnityEngine;
using System.Collections; // Required for Coroutines

public class ZombieSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject zombiePrefab;
    public float spawnInterval = 3f; // Time between spawns

    [Header("Locations")]
    public Transform[] spawnPoints; // Array of places zombies can appear

    void Start()
    {
        // Start the infinite spawning loop
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        // "while(true)" is safe here because it's inside a Coroutine with a timer
        while (true)
        {
            SpawnEnemy();
            // Wait for x seconds before running the loop again
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        // 1. Pick a random spawn point from the list
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        // 2. Spawn the zombie at that position
        Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
    }
}