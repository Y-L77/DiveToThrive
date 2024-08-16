using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    private float moveInput;

    void Start()
    {
        // Find the Animator component on the child object "playersprite(do not change)"
        animator = transform.Find("playersprite(do not change)")?.GetComponent<Animator>();

        // Check if animator was successfully found
        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
        }
    }

    void Update()
    {
        // Get player movement input (adjust axis names based on your setup)
        moveInput = Mathf.Abs(Input.GetAxis("Horizontal")); // or use rb.velocity.x for physics-based movement

        // Set the Speed parameter based on the movement input
        if (animator != null)
        {
            animator.SetFloat("animSpeed", moveInput);
        }
    }
}
