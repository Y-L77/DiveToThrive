using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bobShopScript : MonoBehaviour
{
    public playerMovement PlayerMovement;
    public CashScript cashScript;
    public Button buyOxygenButton;
    void Start()
    {
        if(buyOxygenButton != null)
        {
            buyOxygenButton.onClick.AddListener(buyOxygen);
        }
    }

    public void buyOxygen()
    {
        if(cashScript.playerCash >= 100)
        {
            cashScript.playerCash -= 100;
            PlayerMovement.maxOxygen += 10;
            PlayerMovement.oxygenTime += 10;
        }
        else
        {
            notEnoughCash();
        }
    }


    void Update()
    {
        
    }

    void notEnoughCash()
    {
        Debug.Log("too poor");
    }
}
