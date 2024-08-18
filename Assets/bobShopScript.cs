using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bobShopScript : MonoBehaviour
{
    public playerMovement PlayerMovement;
    public GameObject playerLight;
    public CashScript cashScript;
    public Button buyOxygenButton;
    public Button buyLightButton;
    void Start()
    {
        if(buyOxygenButton != null)
        {
            buyOxygenButton.onClick.AddListener(buyOxygen);
        }
        if(buyLightButton != null)
        {
            buyLightButton.onClick.AddListener(buyLight);
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
    public void buyLight()
    {
        if (cashScript.playerCash >= 250)
        {
            cashScript.playerCash -= 250;
            playerLight.SetActive(true);
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
