using UnityEngine;
using System.Collections;
using System;

public class CellularAutomataWorldgen : MonoBehaviour
{
	public GameObject pfab_chunkManager;
	public int chunkManagerWidth, chunkManagerHeight;

	public string seed;
	public int smoothnessIterationCount;

	[Range(0, 100)]
	public int fillPercentage;

	[Range(0, 100)]
	public int mineralPercentage;

	int[,] smoothedFillMap;
	int[,] smoothedMineralMap;


	Renderer renderer;

	void Start()
	{
		GenerateFillMap();
		GenerateMineralMap();
		InstantiateChunkControllers();
		//InstantiateMap();
	}

	void GenerateFillMap()
	{
		WorldManager.fillMap = new int[WorldManager.width, WorldManager.height];

		System.Random pseudoRandomGenerator = new System.Random(seed.GetHashCode());

		for (int x = 0; x < WorldManager.width; x++)
		{
			for (int y = 0; y < WorldManager.height; y++)
			{
				if (x == 0 || x == WorldManager.width - 1 || y == 0 || y == WorldManager.height - 1)
				{
					WorldManager.fillMap[x, y] = 1; // 1 means the block is filled, 0 means its not.
				}
				else
				{
					WorldManager.fillMap[x, y] = (pseudoRandomGenerator.Next(0, 100) < fillPercentage) ? 1 : 0;
				}
			}
		}

		SmoothFillMap(smoothnessIterationCount);
	}

	void SmoothFillMap(int iterationCount)
	{
		smoothedFillMap = new int[WorldManager.width, WorldManager.height];
		for (int currentIteration = 0; currentIteration < iterationCount; currentIteration++)
		{
			for (int x = 0; x < WorldManager.width; x++)
			{
				for (int y = 0; y < WorldManager.height; y++)
				{
					int neighbourWallTiles = GetSurroundingWallCount(x, y);

					if (neighbourWallTiles > 4)
						smoothedFillMap[x, y] = 1;
					else if (neighbourWallTiles < 4)
						smoothedFillMap[x, y] = 0;

				}
			}
			WorldManager.fillMap = smoothedFillMap;
		}
		
	}

	int GetSurroundingWallCount(int gridX, int gridY)
	{
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
		{
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
			{
				if (neighbourX >= 0 && neighbourX < WorldManager.width && neighbourY >= 0 && neighbourY < WorldManager.height)
				{
					if (neighbourX != gridX || neighbourY != gridY)
					{
						wallCount += WorldManager.fillMap[neighbourX, neighbourY];
					}
				}
				else
				{
					wallCount++;
				}
			}
		}

		return wallCount;
	}

	public void GenerateMineralMap()
	{
		WorldManager.mineralMap = new int[WorldManager.width, WorldManager.height];

		System.Random pseudoRandomGenerator = new System.Random(seed.GetHashCode());

		for (int x = 0; x < WorldManager.width; x++)
		{
			for (int y = 0; y < WorldManager.height; y++)
			{
				if (x == 0 || x == WorldManager.width - 1 || y == 0 || y == WorldManager.height - 1)
				{
					WorldManager.mineralMap[x, y] = 0; // 1 means the block is a mineral block, 0 means its not.
				}
				else if (WorldManager.fillMap[x,y] == 1)
				{
					WorldManager.mineralMap[x, y] = (pseudoRandomGenerator.Next(0, 100) < mineralPercentage) ? 1 : 0;
				}
				else
				{
					WorldManager.mineralMap[x, y] = 0;
				}
			}
		}
	}

	public void InstantiateChunkControllers()
	{
		GameObject lastCreatedObject;
		for (int currentPosX = 0; currentPosX < WorldManager.fillMap.GetUpperBound(0); currentPosX += chunkManagerWidth)
		{
			for (int currentPosY = 0; currentPosY < WorldManager.fillMap.GetUpperBound(1); currentPosY += chunkManagerHeight)
			{
				lastCreatedObject = GameObject.Instantiate(pfab_chunkManager, new Vector3(currentPosX * WorldManager.tileLength, currentPosY * WorldManager.tileLength * (-1), 0), Quaternion.identity, transform);
				ChunkController lastCreatedController = lastCreatedObject.GetComponent<ChunkController>();
				lastCreatedController.InitializeVariables(currentPosX, currentPosX + chunkManagerWidth, currentPosY, currentPosY + chunkManagerHeight);
			}
		}
	}

	public void InstantiateMap()
	{
		Block lastCreatedTile;
		for (int currentPosX = 0; currentPosX < WorldManager.fillMap.GetUpperBound(0); currentPosX++)
		{
			for (int currentPosY = 0; currentPosY < WorldManager.fillMap.GetUpperBound(1); currentPosY++)
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
	public void GenerateTexture()
	{
		renderer = GetComponent<Renderer>();
		Texture2D texture = new Texture2D(WorldManager.width, WorldManager.height);

		for (int currentPosX = 0; currentPosX < WorldManager.width; currentPosX++)
		{
			for (int currentPosY = 0; currentPosY < WorldManager.height; currentPosY++)
			{
				Color color = (WorldManager.fillMap[currentPosX, currentPosY] == 1) ? new Color(0, 0, 0) : new Color(255, 255, 255);
				if (color.r <= 0.0f) // Only way the red channel can be zero is if its a filled square.
				{
					color = (WorldManager.mineralMap[currentPosX, currentPosY] == 1) ? new Color(0, 255, 0) : new Color(0, 0, 0);
				}
				texture.SetPixel(currentPosX, currentPosY, color);
			}
		}
		texture.filterMode = FilterMode.Point;
		texture.Apply();
		renderer.material.mainTexture = texture;
	}
}