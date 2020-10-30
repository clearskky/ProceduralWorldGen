using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Entities;

public class ChunkController : MonoBehaviour, IChunkManager
{
	public int chunkRangeXStart, chunkRangeXEnd;
	public int chunkRangeYStart, chunkRangeYEnd;

	private bool chunkHasBeenInitialized = false;
	private bool chunkHasBeenDisabled = false;

	public void InitializeVariables(int chunkRangeXStart, int chunkRangeXEnd, int chunkRangeYStart, int chunkRangeYEnd)
	{
		this.chunkRangeXStart = chunkRangeXStart;
		this.chunkRangeXEnd = chunkRangeXEnd;
		this.chunkRangeYStart = chunkRangeYStart;
		this.chunkRangeYEnd = chunkRangeYEnd;
	}

	public void CreateChunk()
	{
		if (chunkHasBeenInitialized == false)
		{
			for (int currentPosX = chunkRangeXStart; currentPosX < chunkRangeXEnd; currentPosX++)
			{
				for (int currentPosY = chunkRangeYStart; currentPosY < chunkRangeYEnd; currentPosY++)
				{
					if (WorldManager.fillMap[currentPosX, currentPosY] == 1)
					{
						if (WorldManager.mineralMap[currentPosX, currentPosY] == 1)
						{
							CreateTile(WorldManager.mineralTile, currentPosX, currentPosY);
						}
						else
						{
							CreateTile(WorldManager.dirtTile, currentPosX, currentPosY);
						}
					}
				}
			}
			chunkHasBeenInitialized = true;
		}
	}

	private GameObject CreateTile(GameObject prefabToInstantiate, int currentPosX, int currentPosY)
	{
		GameObject currentTile;
		currentTile = GameObject.Instantiate(prefabToInstantiate, new Vector3(currentPosX * WorldManager.tileLength, currentPosY * WorldManager.tileLength * (-1), 0), Quaternion.identity, transform);
		currentTile.name = "dirtTile_" + currentPosX.ToString() + "_" + currentPosY.ToString();
		Block currentBlock = (Block)currentTile.GetComponent<IBlock>();
		currentBlock.posX = currentPosX;
		currentBlock.posY = currentPosY;
		return currentTile;
	}

	public void DisableAllTiles()
	{
		for (int tileIndex = 0; tileIndex < gameObject.transform.transform.childCount; tileIndex++)
		{
			gameObject.transform.GetChild(tileIndex).gameObject.SetActive(false);
		}
		chunkHasBeenDisabled = true;
	}


	public void EnableAllTiles()
	{
		if (chunkHasBeenDisabled)
		{
			for (int tileIndex = 0; tileIndex < gameObject.transform.transform.childCount; tileIndex++)
			{
				gameObject.transform.GetChild(tileIndex).gameObject.SetActive(true);
			}
		}
		chunkHasBeenDisabled = false;
	}
}
