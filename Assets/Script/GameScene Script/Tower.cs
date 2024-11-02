using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int cost = 75; // Cost of creating a tower

    // Creates a tower at the specified position if the player has enough balance
    public bool CreateTower(GameObject tower, Vector3 position)
    {
        // Find the Bank component in the scene
        Bank bank = FindObjectOfType<Bank>();

        // Check if the Bank component was found
        if (bank == null)
        {
            // If the Bank component is not found, return false (tower creation failed)
            return false;
        }

        // Check if the player has enough balance to create the tower
        if (bank.CurrentBalance >= cost)
        {
            // If the player has enough balance, instantiate the tower at the specified position and orientation
            Instantiate(tower, position, Quaternion.identity);

            // Withdraw the cost from the player's balance
            bank.WithDrawal(cost);

            // Indicate that the tower creation was successful
            return true;
        }

        // If the player doesn't have enough balance, return false (tower creation failed)
        return false;
    }
}
