using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class harpoonArrowDestroy : MonoBehaviour
{

    private void Start()
    {
        Destroy(gameObject, 5f);
        Debug.Log("spawn destroy");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject, 0.3f);
        Debug.Log("collider destroy");
    }
}
