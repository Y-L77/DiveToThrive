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
    public Camera camera;
    public GameObject waterOverlay;
    public GameObject oxygenFill; //reference to the fill layer in the oxygen bar.
    public int oxygenTime; // variable for how long the player can hold breathe
    public int maxOxygen;
    public float drownTime = 2;
    public float elapsedTime = 0;
    public AudioSource diveSound;
    public AudioSource underwaterSound;

    private void Update() //add coyote jump to this later
    {
        waterOverlay.SetActive(submerged); //submerged is a boolean value, if is submerged it will appear

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
                camera.orthographicSize = 9;
            }
        }
        if(submerged)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime > drownTime)
            {
                underwaterSound.Play();
                updateOxygen();
                elapsedTime = 0;
            }

        }

        if (oxygenTime <= 0)
        {
            drown();
        }
    }

    private void FixedUpdate() //swimming is physics heavy so this method will be used to get swimming inputs
    {
        if (submerged)
        {
            cameraFollowPlayer();
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector2 movement = new Vector2(moveHorizontal, moveVertical);

            // Adjust the multiplier based on how strong you want the swimming force to be
            Rigidbody2D.AddForce(movement * swimForce);

            float rotationSpeed = 8f;
            float angle = Mathf.Atan2(Rigidbody2D.velocity.y, Rigidbody2D.velocity.x) * Mathf.Rad2Deg;

            if (Rigidbody2D.velocity.magnitude > 0.1f)
            { // Only rotate when moving
                Rigidbody2D.rotation = Mathf.LerpAngle(Rigidbody2D.rotation, angle, rotationSpeed * Time.fixedDeltaTime);
            }
        }
        else if(!submerged)
        {
            camera.transform.position = new Vector3(0, 0, -10);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground")) // Check if the collided object is tagged as "ground"
        {
            isGrounded = true; // Set grounded status to false
            oxygenTime = maxOxygen;
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
            diveSound.Play();
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
    void cameraFollowPlayer() //i used chatgpt for this because tutorials had the script in the camera and it would of took meaningless time
    {
        camera.orthographicSize = 6;
        // Get the player's position
        Vector3 targetPosition = player.transform.position;

        // Set the target position's z to -10 to maintain the camera's z distance
        targetPosition.z = -10;

        // Smoothly interpolate between the current camera position and the target position
        camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition, 0.1f);
    }

    void updateOxygen()
    {
        oxygenTime--;
        // Update the oxygen fill UI
        float fillPercentage = (float)oxygenTime / maxOxygen; // Calculate the fill percentage
        float leftValue = Mathf.Lerp(750, 270, fillPercentage); // Calculate the left value based on fill percentage

        // Update the RectTransform of the oxygenFill
        RectTransform oxygenFillRect = oxygenFill.GetComponent<RectTransform>();
        oxygenFillRect.offsetMin = new Vector2(leftValue, oxygenFillRect.offsetMin.y);
    }

    void drown()
    {
        Debug.Log("you drowned");
        //make logic for what happens when the player drowns
        oxygenTime = maxOxygen;
    }

    
}

