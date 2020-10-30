using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;

public class DynamicCameraOcclusion : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D chunk) // Calling it chunk because Camera can only collide with chunk controllers
    {
        Debug.Log("Goodnight");
        JobHandle jobHandle = chunk.gameObject.GetComponent<ChunkController>().DisableAllTiles();
        jobHandle.Complete();
    }

    private void OnTriggerEnter2D(Collider2D chunk)
    {
        Debug.Log("Hello ");
        JobHandle jobHandle = chunk.GetComponent<ChunkController>().EnableAllTiles();
        jobHandle.Complete();
    }
}
