using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class swordfishScript : MonoBehaviour
{
    public int fishValue; //how much coins the fish gives

    public float speed = 2f; // Speed of the fish
    public float wanderRadiusX = 2f; // X-axis wander range
    public float wanderRadiusY = 4f; // Y-axis wander range (within 4-5 units)
    public float detectionRange = 10f; // Distance at which the fish detects the player
    public bool isTouchingPlayer = false;
    public bool fishAlive = true;
    public string fishName; //used for the fish text on catch
    private Rigidbody2D rb;
    private Vector2 startPosition;
    private Vector2 wanderTarget;
    public bool fleeing = false;
    private bool isInWater = false; // Track if the fish is in water

    private playerMovement playerMovementScript; // Reference to the player's movement script
    private CashScript cashScript;
    private catchFish CatchFish;
    public AudioSource fishFleeing;
    private bool isSpinning = false; // Flag for spinning
    public TMP_Text catchText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        SetWanderTarget();

        GameObject player = GameObject.FindGameObjectWithTag("player");
        if (player != null)
        {
            playerMovementScript = player.GetComponent<playerMovement>();
            CatchFish = player.GetComponent<catchFish>();
        }
        GameObject cashObject = GameObject.FindGameObjectWithTag("cash");
        if (cashObject != null)
        {
            cashScript = cashObject.GetComponent<CashScript>();
            fishFleeing = cashObject.GetComponent<AudioSource>();
        }
        GameObject textObject = GameObject.FindGameObjectWithTag("text");
        if (textObject != null)
        {
            catchText = textObject.GetComponent<TMP_Text>();
        }
    }

    void Update()
    {
        if (isSpinning)
        {
            SpinFish(); // Call SpinFish if the fish is spinning
        }

        if (isInWater) // Only update behavior if in water
        {
            if (fleeing)
            {
                Flee();
            }
            else
            {
                Wander();
                DetectPlayer();
            }
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop movement if not in water
        }

        if (playerMovementScript.shifting)
        {
            fleeing = false;
        }

        // Method of catching the fish
        if (CatchFish.hands == true)
        {
            if (isTouchingPlayer && Input.GetKeyDown(KeyCode.Mouse0))
            {
                catchFish();
            }
        }

    }

    void Wander()
    {
        // If the fish has reached the wander target, set a new one
        if (Vector2.Distance(transform.position, wanderTarget) < 0.1f || rb.velocity.magnitude < 0.1f)
        {
            SetWanderTarget();
        }

        // Calculate direction towards the wander target
        Vector2 direction = (wanderTarget - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed;

        // Rotate the fish towards the direction of movement
        RotateTowards(direction);
    }

    void SetWanderTarget()
    {
        // Set a new random wander target within the defined range
        float wanderX = Random.Range(startPosition.x - wanderRadiusX, startPosition.x + wanderRadiusX);
        float wanderY = Random.Range(startPosition.y - wanderRadiusY, startPosition.y + wanderRadiusY);
        wanderTarget = new Vector2(wanderX, wanderY);

        // Ensure the fish starts moving towards the new target immediately
        Vector2 direction = (wanderTarget - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed;
    }

    void DetectPlayer()
    {
        if (playerMovementScript != null && !playerMovementScript.shifting)
        {
            GameObject player = GameObject.FindGameObjectWithTag("player");
            if (player != null)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance < detectionRange)
                {
                    fleeing = true;
                }
            }
        }
    }

    void Flee() //chasing now for the swordfish
    {
        GameObject player = GameObject.FindGameObjectWithTag("player");
        if (player != null)
        {
            if (!fishFleeing.isPlaying) // Check if the sound is not already playing
            {
                fishFleeing.Play();
            }

            Vector2 chaseDirection = (player.transform.position - transform.position).normalized;
            rb.velocity = chaseDirection * speed * 2.5f; // Increase speed while chasing

            // Stop chasing once the fish is close enough to the player
            if (Vector2.Distance(transform.position, player.transform.position) < 1f) // Adjust this distance as needed
            {
                fleeing = false;
                rb.velocity = Vector2.zero; // Stop movement when the fish reaches the player

                // Stop the chasing sound when the fish stops chasing
                fishFleeing.Stop();
            }

            // Rotate the fish towards the chasing direction
            RotateTowards(chaseDirection);
        }
    }

    void RotateTowards(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("water"))
        {
            isInWater = true; // Fish enters the water
        }
        if (collision.CompareTag("harpoon"))
        {
            catchFish();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("water"))
        {
            isInWater = false; // Fish exits the water
            rb.velocity = Vector2.zero; // Stop moving when leaving the water
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            AdjustDirectionOnCollision(collision.contacts[0].normal);
        }
        if (collision.gameObject.CompareTag("player"))
        {
            isTouchingPlayer = true; // Fish is touching the player
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            isTouchingPlayer = false; // Fish is no longer touching the player
        }
    }

    void AdjustDirectionOnCollision(Vector2 collisionNormal)
    {
        // Reflect the current velocity along the collision normal to move away from the obstacle
        Vector2 reflectedVelocity = Vector2.Reflect(rb.velocity, collisionNormal);
        rb.velocity = reflectedVelocity.normalized * speed;
    }

    void catchFish()
    {
        if (fishAlive)
        {
            StartCoroutine(catchTextChange());
            fishAlive = false;
            speed = 0;
            isSpinning = true; // Start spinning
            cashScript.playerCash += fishValue;
            Destroy(gameObject, 1.5f);

            // Implement showing the coin UI later
        }
    }

    void SpinFish()
    {
        // Rotate the fish around its Z-axis
        transform.Rotate(new Vector3(0, 0, 360) * Time.deltaTime);
    }

    public IEnumerator catchTextChange()
    {
        catchText.text = "You caught a " + fishName + "! + " + fishValue;
        yield return new WaitForSeconds(1.4f);
        catchText.text = "";

    }
}
