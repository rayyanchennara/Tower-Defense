using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 150; // Initial balance the player starts with
    int currentBalance; // Current balance of the player
    public int CurrentBalance { get { return currentBalance; } } // Public property to access the current balance

    [SerializeField] TextMeshProUGUI score; // Reference to the UI text that displays the current balance

    // Start is called before the first frame update
    void Awake()
    {
        // Initialize the current balance with the starting balance
        currentBalance = startingBalance;

        // Update the UI text to display the initial balance
        UpdateScore();
    }

    // Updates the UI text to display the current balance
    private void UpdateScore()
    {
        // Check if the score text object is not null to avoid errors
        if (score == null) { return; }

        // Set the text of the score UI to the current balance
        score.text = "" + currentBalance.ToString();
    }

    // Withdraws a specified amount from the current balance
    public void WithDrawal(int amount)
    {
        // Subtract the absolute value of the amount from the current balance
        currentBalance -= Mathf.Abs(amount);

        // Check if the balance is below zero and reload the scene if it is
        CheckBalance();

        // Update the UI text to display the new balance
        UpdateScore();
    }

    // Deposits a specified amount to the current balance
    public void Deposit(int amount)
    {
        // Add the absolute value of the amount to the current balance
        currentBalance += Mathf.Abs(amount);

        // Check if the balance is below zero and reload the scene if it is
        CheckBalance();

        // Update the UI text to display the new balance
        UpdateScore();
    }

    // Checks if the current balance is below zero and reloads the scene if it is
    private void CheckBalance()
    {
        if (currentBalance <= 0)
        {
            // Reload the scene with index 0 (presumably the main menu or game over scene)
            ReloadScene(2);
        }
    }

    // Reloads the specified scene
    private void ReloadScene(int intexNumber)
    {
        SceneManager.LoadScene(intexNumber);
    }
}
