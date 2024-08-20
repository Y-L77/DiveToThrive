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
    public Button buyRadarButton;
    public GameObject ylevel;
    public catchFish CatchFish;
    public Button buyHarpoonButton;
    public GameObject bloopTrail;
    public Button buyBloopButton;
    public AudioSource buySFX;
    public AudioSource notenoughSFX;
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
        if (buyRadarButton != null)
        {
            buyRadarButton.onClick.AddListener(buyRadar);
        }
        if (buyHarpoonButton != null)
        {
            buyHarpoonButton.onClick.AddListener(buyHarpoon);
        }
        if (buyBloopButton != null)
        {
            buyBloopButton.onClick.AddListener(buyBloop);
        }
    }

    public void buyOxygen()
    {
        if(cashScript.playerCash >= 100)
        {
            cashScript.playerCash -= 100;
            PlayerMovement.maxOxygen += 10;
            PlayerMovement.oxygenTime += 10;
            buySFX.Play();
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
            buySFX.Play();
        }
        else
        {
            notEnoughCash();
        }
    }
    public void buyRadar()
    {
        if(cashScript.playerCash >= 400)
        {
            cashScript.playerCash -= 400;
            ylevel.SetActive(true);
            buySFX.Play();
        }
        else
        {
            notEnoughCash();
        }
    }
    public void buyHarpoon()
    {
        if (cashScript.playerCash >= 750)
        {
            cashScript.playerCash -= 750;
            CatchFish.harpoon = true;
            buySFX.Play();
        }
        else
        {
            notEnoughCash();
        }
    }
    public void buyBloop()
    {
        if (cashScript.playerCash >= 888)
        {
            cashScript.playerCash -= 888;
            bloopTrail.SetActive(true);
            buySFX.Play();
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
        notenoughSFX.Play();
    }
}
