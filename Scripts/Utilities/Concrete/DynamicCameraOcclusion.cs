using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCameraOcclusion : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D chunk) // Calling it chunk because Camera can only collide with chunk controllers
    {
        Debug.Log("Goodnight");
        chunk.gameObject.GetComponent<ChunkController>().DisableAllTiles();
    }

    private void OnTriggerEnter2D(Collider2D chunk)
    {
        Debug.Log("Hello ");
        chunk.GetComponent<ChunkController>().EnableAllTiles();
    }
}
