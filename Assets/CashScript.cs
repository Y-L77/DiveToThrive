using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CashScript : MonoBehaviour
{
    public float playerCash;
    public Text cashText;

    private void Update()
    {
        UpdateCashText();
    }

    private void UpdateCashText()
    {
        cashText.text = playerCash.ToString();
    }
}
