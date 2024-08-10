using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CashScript : MonoBehaviour
{
    public double playerCash;
    public Text cashText;

    private void OnValidate()
    {
        UpdateCashText();
    }

    // Start is called before the first frame update
    private void Start()
    {
        UpdateCashText();
    }

    private void Update()
    {

    }

    private void UpdateCashText()
    {
        cashText.text = playerCash.ToString();
    }
}
