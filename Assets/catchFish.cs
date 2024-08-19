using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//code by yipeng
public class catchFish : MonoBehaviour
{
    public bool hands = true;
    public bool harpoon = false;
    public GameObject harpoonItem;
    public GameObject arrowPrefab; // Reference to the arrow prefab
    public Transform firePoint; // The point where the arrow will be instantiated

    private bool canShoot = true;
    public float cooldownTime = 3f;

    private void Update()
    {
        if (harpoon)
        {
            harpoonItem.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0) && canShoot)
            {
                ShootArrow();
            }
        }
        else
        {
            harpoonItem.SetActive(false);
        }
    }

    void ShootArrow()
    {
        // Create the arrow instance
        GameObject arrowInstance = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

        // Get the direction of the mouse relative to the fire point
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)firePoint.position;
        direction.Normalize();

        // Rotate the arrow to face the direction of the mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrowInstance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Add force to the arrow to shoot it in the desired direction
        Rigidbody2D rb = arrowInstance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * 10f; // Adjust the speed as necessary
        }

        // Start cooldown
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldownTime);
        canShoot = true;
    }
}
