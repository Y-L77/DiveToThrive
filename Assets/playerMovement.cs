using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public Rigidbody2D Rigidbody2D;
    public GameObject player;
    public bool isGrounded = false;
    private float jumpHeight = 5.5f;
    public bool submerged = false;
    public float speed = 4f;
    public float swimForce = 10f;

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
                player.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (moveInput > 0) // Move right
            {
                player.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    private void FixedUpdate() //swimming is physics heavy so this method will be used to get swimming inputs
    {
        if (submerged)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector2 movement = new Vector2(moveHorizontal, moveVertical);

            // Adjust the multiplier based on how strong you want the swimming force to be
            Rigidbody2D.AddForce(movement * swimForce);

            float rotationSpeed = 2f;
            float angle = Mathf.Atan2(Rigidbody2D.velocity.y, Rigidbody2D.velocity.x) * Mathf.Rad2Deg;

            if (Rigidbody2D.velocity.magnitude > 0.1f)
            { // Only rotate when moving
                Rigidbody2D.rotation = Mathf.LerpAngle(Rigidbody2D.rotation, angle, rotationSpeed * Time.fixedDeltaTime);
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("water"))
        {
            submerged = true;
            player.transform.rotation = Quaternion.Euler(0, 0, -135);
            Rigidbody2D.drag = 5f;
            Rigidbody2D.angularDrag = 10f;
            Rigidbody2D.gravityScale = 0.1f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("water"))
        {
            submerged = false;
            Rigidbody2D.drag = 0f;
            Rigidbody2D.angularDrag = 0.05f;
            Rigidbody2D.gravityScale = 1f;
            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, Rigidbody2D.velocity.y * 3f);
        }
    }
}

