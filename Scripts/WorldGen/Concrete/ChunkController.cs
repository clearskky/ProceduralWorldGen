using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Entities;

public class ChunkController : MonoBehaviour, IChunkManager
{
	public GameObject tileContainer;

	public int chunkRangeXStart, chunkRangeXEnd;
	public int chunkRangeYStart, chunkRangeYEnd;

	private bool chunkHasBeenInitialized = false;
	private bool chunkHasBeenDisabled = false;

	public void Awake()
	{
		tileContainer = transform.GetChild(0).gameObject; // First and the only child after instantiation is the tileContainer
	}
	public void InitializeVariables(int chunkRangeXStart, int chunkRangeXEnd, int chunkRangeYStart, int chunkRangeYEnd)
	{
		this.chunkRangeXStart = chunkRangeXStart;
		this.chunkRangeXEnd = chunkRangeXEnd;
		this.chunkRangeYStart = chunkRangeYStart;
		this.chunkRangeYEnd = chunkRangeYEnd;
	}

	public IEnumerator CreateChunk()
	{
		if (chunkHasBeenInitialized == false)
		{
			for (int currentPosX = chunkRangeXStart; currentPosX < chunkRangeXEnd; currentPosX++)
			{
				for (int currentPosY = chunkRangeYStart; currentPosY < chunkRangeYEnd; currentPosY++)
				{
					if (WorldManager.fillMap[currentPosX, currentPosY] == 1)
					{
						CreateTile(WorldManager.baseBlock, currentPosX, currentPosY, WorldManager.mineralMap[currentPosX, currentPosY]);
						yield return null;
					}
					
				}
			}
			chunkHasBeenInitialized = true;
		}

	}

	private GameObject CreateTile(GameObject prefabToInstantiate, int currentPosX, int currentPosY, int blockId)
	{
		GameObject currentTile;
		currentTile = GameObject.Instantiate(prefabToInstantiate, new Vector3(currentPosX * WorldManager.tileLength, 
																			  currentPosY * WorldManager.tileLength * (-1), 0), 
																			  Quaternion.identity, tileContainer.transform);
		currentTile.name = "tile_" + currentPosX.ToString() + "_" + currentPosY.ToString();
		Block currentBlock = (Block)currentTile.GetComponent<IBlock>();
		currentBlock.posX = currentPosX;
		currentBlock.posY = currentPosY;
		currentBlock.FeedBlockData(WorldManager.GetBlockDataOfSpecificId(blockId));
		return currentTile;
	}

	// Unused Overload
	private GameObject _CreateTile(GameObject prefabToInstantiate, int currentPosX, int currentPosY)
	{
		GameObject currentTile;
		currentTile = GameObject.Instantiate(prefabToInstantiate, new Vector3(currentPosX * WorldManager.tileLength, currentPosY * WorldManager.tileLength * (-1), 0), Quaternion.identity, tileContainer.transform);
		currentTile.name = "dirtTile_" + currentPosX.ToString() + "_" + currentPosY.ToString();
		Block currentBlock = (Block)currentTile.GetComponent<IBlock>();
		currentBlock.posX = currentPosX;
		currentBlock.posY = currentPosY;
		return currentTile;
	}

	public IEnumerator DisableAllTiles()
	{
		tileContainer.SetActive(false);
		chunkHasBeenDisabled = true;
		yield return null;
	}


	public IEnumerator EnableAllTiles()
	{
		if (chunkHasBeenDisabled)
		{
			tileContainer.SetActive(true);
			yield return null;
		}
		else if (chunkHasBeenDisabled == false && chunkHasBeenInitialized == false)
		{
			StartCoroutine(CreateChunk());
		}
		chunkHasBeenDisabled = false;
		
	}
}
