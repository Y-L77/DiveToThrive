using UnityEngine;

public class FishAI : MonoBehaviour
{
    public int fishValue; //how much coins the fish gives



    public float speed = 2f; // Speed of the fish
    public float wanderRadiusX = 2f; // X-axis wander range
    public float wanderRadiusY = 4f; // Y-axis wander range (within 4-5 units)
    public float detectionRange = 10f; // Distance at which the fish detects the player
    public bool isTouchingPlayer = false;
    public bool fishAlive = true;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private Vector2 wanderTarget;
    public bool fleeing = false;
    private bool isInWater = false; // Track if the fish is in water

    private playerMovement playerMovementScript; // Reference to the player's movement script
    private CashScript cashScript;
    private catchFish CatchFish;

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
        }
    }

    void Update()
    {
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




        //method of catching the fish
        if(CatchFish.hands == true)
        {
            if (isTouchingPlayer && Input.GetKeyDown(KeyCode.Mouse0))
            {
                catchFish();
            }
        }
    }

    void Wander()
    {
        if (Vector2.Distance(transform.position, wanderTarget) < 0.1f)
        {
            SetWanderTarget();
        }

        Vector2 direction = (wanderTarget - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed;

        if (direction.x > 0 && transform.localScale.x < 0)
            Flip();
        else if (direction.x < 0 && transform.localScale.x > 0)
            Flip();
    }

    void SetWanderTarget()
    {
        float wanderX = Random.Range(startPosition.x - wanderRadiusX, startPosition.x + wanderRadiusX);
        float wanderY = Random.Range(startPosition.y - wanderRadiusY, startPosition.y + wanderRadiusY);
        wanderTarget = new Vector2(wanderX, wanderY);
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

    void Flee()
    {
        GameObject player = GameObject.FindGameObjectWithTag("player");
        if (player != null)
        {
            Vector2 fleeDirection = (transform.position - player.transform.position).normalized;
            rb.velocity = fleeDirection * speed * 2; // Increase speed while fleeing

            if (Vector2.Distance(transform.position, player.transform.position) > detectionRange)
            {
                fleeing = false;
                rb.velocity = Vector2.zero;
                SetWanderTarget();
            }

            if (fleeDirection.x > 0 && transform.localScale.x < 0)
                Flip();
            else if (fleeDirection.x < 0 && transform.localScale.x > 0)
                Flip();
        }
    }

    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("water"))
        {
            isInWater = true; // Fish enters the water
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
            wanderTarget.x = -wanderTarget.x; // Go the other way on X-axis
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
            isTouchingPlayer = false; // Fish no touch
        }
    }

    void catchFish()
    {
        if(fishAlive)
        {
            fishAlive = false;
            Destroy(gameObject, 1.5f);
            speed = 0;
            cashScript.playerCash += fishValue;


            //implemet show the coin ui later


        }



    }
}
