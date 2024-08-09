using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public Rigidbody2D Rigidbody2D;
    public GameObject player;
    public bool isGrounded = false;
    private float jumpHeight = 5.5f;
    public bool submerged = false;
    public float speed = 4f;

    private void Update() //add coyote jump to this later
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) //this script is for the player on land, not in water.
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

        float moveInput = Input.GetAxis("Horizontal");
        if (!submerged)
        {
            Rigidbody2D.velocity = new Vector2(moveInput * speed, Rigidbody2D.velocity.y); // Handle horizontal movement

            // Flip the player sprite based on movement direction
            if (moveInput < 0) // Move left
            {
                player.transform.localScale = new Vector3(-0.5f, 0.5f, 1); // Flip left
            }
            else if (moveInput > 0) // Move right
            {
                player.transform.localScale = new Vector3(0.5f, 0.5f, 1); // Flip right
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground")) // Check if the collided object is tagged as "ground"
        {
            isGrounded = true; // Set grounded status to false
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground")) // Check if the collided object is tagged as "ground"
        {
            isGrounded = false; // Set grounded status to false
        }
    }
}

