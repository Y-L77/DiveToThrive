using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airBubbleScript : MonoBehaviour
{
    public playerMovement playermovement;
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("player");
        if (player != null)
        {
            playermovement = player.GetComponent<playerMovement>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(playermovement.oxygenTime < playermovement.maxOxygen)
        {
            playermovement.oxygenTime += playermovement.maxOxygen / 10;
        }
        else
        {
            Destroy(gameObject, 0.2f);
        }

        Destroy(gameObject, 0.2f);
    }
}

