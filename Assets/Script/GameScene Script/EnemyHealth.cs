using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    int maxHitPoints = 5; // Maximum hit points the enemy will destroy
    int currentHitPoints = 0; // Current hit points of the enemy
    int difficultyPoint = 2; // Amount by which max hit points increase on every death of enemy

    Enemy enemy; // Reference to the Enemy component

    // Start is called before the first frame update
    void Start()
    {
        // Get the Enemy script attached to this game object
        enemy = GetComponent<Enemy>();
    }

    // Called when a particle collides with the enemy
    private void OnParticleCollision(GameObject other)
    {
        // Process the hit
        ProcessHit();
    }

    // Processes a hit on the enemy
    private void ProcessHit()
    {
        // Increase the current hit points
        currentHitPoints++;

        // Check if the hit points exceed max hit points
        if (currentHitPoints >= maxHitPoints)
        {
            // Process for destroy enemy
            ProcessKill();
        }
    }

    private void ProcessKill()
    {
        // Reward points to the player
        enemy.RewardGold();

        // Deactivate this game object or destroy enemy
        gameObject.SetActive(false);

        // Reset current hit points and increase max hit points for the next enemy
        currentHitPoints = 0;
        maxHitPoints += difficultyPoint;
    }
}
