using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] float maxDistance; // Maximum distance for target detection
    [SerializeField] float range = 20f; // Range at which the weapon can attack
    Transform target; // Target enemy transform
    [SerializeField] GameObject weapon; // GameObject representing the weapon
    [SerializeField] ParticleSystem arrow; // Particle system for the weapon's attack effect

    // Update is called once per frame
    void Update()
    {
        // Find the closest enemy within range
        FindCloseEnemy();

        // Aim the weapon at the target and determine if it's within range to attack
        AimWeapon();
    }

    // Finds the closest enemy within range
    private void FindCloseEnemy()
    {
        // Find all enemy objects in the scene
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        // Initialize the closest target and its distance
        Transform closeTarget = null;
        maxDistance = Mathf.Infinity;

        // Iterate through all enemies
        foreach (Enemy enemy in enemies)
        {
            // Calculate the distance to the enemy
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            // If the enemy is closer than the current closest target, update the closest target
            if (targetDistance < maxDistance)
            {
                closeTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }

        // Set the target to the closest enemy found
        target = closeTarget;
    }

    // Aims the weapon at the target and determines if it's within range to attack
    private void AimWeapon()
    {
        // Rotate the weapon to look at the target
        weapon.transform.LookAt(target);

        // Check if the target is within range
        if (maxDistance < range)
        {
            // Activate the attack effect (e.g., particle system)
            Attack(true);
        }
        else
        {
            // Deactivate the attack effect
            Attack(false);
        }
    }

    // Activates or deactivates the attack effect based on the given isActive flag
    private void Attack(bool isActive)
    {
        // Get the emission module of the particle system
        var emissionModule = arrow.emission;

        // Enable or disable the emission module based on the isActive flag
        emissionModule.enabled = isActive;
    }
}
