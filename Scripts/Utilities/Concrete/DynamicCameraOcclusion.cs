using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCameraOcclusion : MonoBehaviour
{
    private void OnTriggerLeave2D(Collider2D collision)
    {
        Destroy(collision.gameObject.GetComponent<BoxCollider2D>());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BoxCollider2D rb = other.gameObject.AddComponent<BoxCollider2D>();
        rb.size = new Vector2(10, 10);
        //rb.bodyType = RigidbodyType2D.Static;
    }
}
