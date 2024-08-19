using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CashScript : MonoBehaviour
{
    public int playerCash;
    public Text cashText;

    private void Start()
    {
        // Load player cash when the game starts
        playerCash = PlayerPrefs.GetInt("PlayerCash", 0);
        UpdateCashText();
    }

    private void Update()
    {
        UpdateCashText();
    }

    private void UpdateCashText()
    {
        cashText.text = playerCash.ToString("F0"); // Display cash as a floating-point number with 2 decimals
    }

    public void AddCash(int amount)
    {
        playerCash += amount;
        SaveCash(); // Save the cash whenever it changes
        UpdateCashText();
    }

    private void SaveCash()
    {
        PlayerPrefs.SetInt("PlayerCash", playerCash);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        // Save the cash when the application is quitting
        SaveCash();
    }
}
