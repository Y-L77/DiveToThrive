using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class harpoonArrowDestroy : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 8f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the projectile has collided with an object tagged as "Fish" or "Ground"
        if (collision.CompareTag("fish") || collision.CompareTag("ground"))
        {
            Destroy(gameObject); // Destroy the projectile
            Debug.Log("Projectile destroyed upon hitting " + collision.tag);
        }
    }

}
