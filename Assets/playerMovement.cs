using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public Rigidbody2D Rigidbody2D;
    public bool isGrounded;
    private float jumpHeight = 5.5f;


    private void Update() //add coyote jump to this later
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            if(isGrounded)
            {
                Rigidbody2D.velocity = Vector2.up * jumpHeight;
            }
            else
            {
                Debug.Log("not on ground yet");
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ground")) //freeze rigidbody velocity and gravity so it doesnt keep falling when grounded, 
        {
            Rigidbody2D.gravityScale = 0;
            Rigidbody2D.velocity = Vector2.zero;
            isGrounded = true; //variable change so when on ground you can jump
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            Rigidbody2D.gravityScale = 1;
            isGrounded = false; //variable change so when on ground you can jump
        }
    }
}

