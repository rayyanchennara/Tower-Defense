using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab; // Prefab of the enemy object to instantiate
    [SerializeField] float activateTimer = 3f; // Time interval between enemy activates
    [SerializeField] int poolSize = 5; // Maximum number of enemies in the pool

    GameObject[] pool; // Array to store the pooled enemy objects

    // Awake is called before the first frame update
    private void Awake()
    {
        // Populate the pool with inactive enemy objects
        PopulatePool();
    }

    // Start is called after Awake
    private void Start()
    {
        // Start the coroutine to spawn enemies periodically
        StartCoroutine(SpawnEnemy());
    }

    // Coroutine to spawn enemies periodically
    IEnumerator SpawnEnemy()
    {
        while (true) // Infinite loop to continuously spawn enemies
        {
            // Enable a deactivated enemy from the pool
            EnableObjectInPool();

            // Wait for the specified spawn timer before spawning the next enemy
            yield return new WaitForSeconds(activateTimer);
        }
    }

    // Populates the pool with inactive enemy objects
    void PopulatePool()
    {
        // Create an array to store the pooled objects
        pool = new GameObject[poolSize];

        // Instantiate the enemy prefab for each slot in the pool
        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(enemyPrefab, transform); // Instantiate the prefab as a child of this object
            pool[i].SetActive(false); // Initially deactivate the enemy object
        }
    }

    // Enables the first inactive enemy object in the pool
    void EnableObjectInPool()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (!pool[i].activeInHierarchy) // Check if the enemy is inactive
            {
                pool[i].SetActive(true); // Activate the enemy
                return; // Exit the loop after activating the first inactive enemy
            }
        }
    }
}

