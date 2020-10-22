using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCameraOcclusion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject.GetComponent<Rigidbody2D>());
    }

    private void OnTriggerLeave2D(Collider2D other)
    {
        Rigidbody2D rb = other.gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
    }
}
