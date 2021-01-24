using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;

public class DynamicCameraOcclusion : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D chunk) // Calling it chunk because Camera can only collide with chunk controllers
    {
        StartCoroutine(chunk.gameObject.GetComponent<ChunkController>().DisableAllTiles());
    }

    private void OnTriggerEnter2D(Collider2D chunk)
    {
        //Debug.Log("Enable tiles");
        StartCoroutine(chunk.GetComponent<ChunkController>().EnableAllTiles());
    }
}
