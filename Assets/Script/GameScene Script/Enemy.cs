using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    int goldReward = 25; // Amount of reward that get player when he destroy enemy
    int goldPenalty = 25; // Amount of penalize that substract from player when enemy pass finishing point

    Bank bank; // Reference to the Bank script

    // Start is called before the first frame update
    void Start()
    {
        // Find the Bank script in the scene
        bank = FindObjectOfType<Bank>();
    }

    // Rewards the player with gold by calling the Bank's Deposit method
    public void RewardGold()
    {
        if (bank == null) { return; } // Check if Bank script is found

        // Deposit the reward amount to the Bank
        bank.Deposit(goldReward);
    }

    // Penalizes the player by calling the Bank's Withdrawal method
    public void StealGold()
    {
        if (bank == null) { return; } // Check if Bank script is found

        // Withdraw the penalty amount from the Bank
        bank.WithDrawal(goldPenalty);
    }
}
