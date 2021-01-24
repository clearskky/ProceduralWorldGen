using UnityEngine;
using System.Collections;
using System;
using Newtonsoft.Json;

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

	public float bronzeWeight, bronzeWeightDepthMult;
	public float ironWeight, ironWeightDepthMult;
	public float silverWeight, silverWeightDepthMult;
	public float goldWeight, goldWeightDepthMult;
	public float jadeWeight, jadeWeightDepthMult;
	public float lithiumWeight, lithiumWeightDepthMult;
	public float uraniumWeight, uraniumWeightDepthMult;
	public float newagWeight, newagWeightDepthMult;
	private float bedrockWeight = 0;


	int[,] smoothedFillMap;
	//int[,] smoothedMineralMap;

	Renderer renderer;

	public void InitializeWorld()
	{
		GenerateFillMap();
		GenerateMineralMap();
		//Debug.Log("Fill and Mineral maps have been initialized for the first time, saving them to playerprefs.");
	}

	public void InstantiateWorld()
	{
		InstantiateChunkControllers();
		//InstantiateMap();

		if (PlayerPrefs.GetInt("worldHasBeenInitialized", 0) != 1)
		{
			Debug.Log("World has been initialized for the first time, saving to playerPrefs.");
			PlayerPrefs.SetInt("worldHasBeenInitialized", 1);
			PlayerPrefs.Save();
		}
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

		Debug.Log("Saving Fill Map.");
		PlayerPrefs.SetString("fillMap", JsonConvert.SerializeObject(WorldManager.fillMap));
		PlayerPrefs.Save();
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
				if (x == WorldManager.width - 1 || x == 0 || y == WorldManager.height - 1 || (x <= 20 && y == 0) || (x >= 25 && y == 0)) // Is the block on the bottom or the sides?
				{
					WorldManager.mineralMap[x, y] = 51; // 51 is the id of the bedrock which cannot be mined
				}
				else if (WorldManager.fillMap[x,y] == 1) // There is a block here
				{
					if((pseudoRandomGenerator.Next(0, 100) < mineralPercentage) ? true : false)
					{
						WorldManager.mineralMap[x, y] = DetermineMineralType(y);
					}
					else // Its not a mineral, give it the id of a DirtBlock, which is 0 
					{
						WorldManager.mineralMap[x, y] = 0;
					}
				}
				else // This spot on the grid isn't filled at all
				{
					WorldManager.mineralMap[x, y] = 0;
				}
			}
		}

		PlayerPrefs.SetString("mineralMap", JsonConvert.SerializeObject(WorldManager.mineralMap));
		PlayerPrefs.Save();
	}

	private int DetermineMineralType(int currentDepth)
	{
		float[] weights = new float[] // The array index corresponds to a block id
		{
			0, // The algorithm will automatically pass this part because no mineral has the id of 0, id of bronze starts from 1
			bronzeWeight    + bronzeWeightDepthMult * currentDepth,
			ironWeight      + ironWeightDepthMult * currentDepth,
			silverWeight    + silverWeightDepthMult * currentDepth,
			goldWeight      + goldWeightDepthMult * currentDepth,
			jadeWeight      + jadeWeightDepthMult * currentDepth,
			lithiumWeight   + lithiumWeightDepthMult * currentDepth,
			uraniumWeight   + uraniumWeightDepthMult * currentDepth,
			newagWeight     + newagWeightDepthMult * currentDepth
		};

		for (int index = 0; index < weights.Length; index++) // Weights cannot be less than 0
		{
			if (weights[index] <= 0f)
			{
				weights[index] = 0f;
			}
		}

		// Get the total sum of all the weights.
		float totalWeight = 0;
		for (int i = 0; i < weights.Length; ++i)
		{
			totalWeight += weights[i];
		}

		
		// Step through all the possibilities, one by one, checking to see if each one is selected.
		for (int weightIndex = 0; weightIndex < weights.Length; weightIndex++)
		{
			// Do a probability check with a likelihood of weights[index] / weightSum.
			if (weights[weightIndex] > UnityEngine.Random.Range(0, totalWeight))
			{
				return weightIndex;
			}
			else
			{
				// Remove the last item from the sum of total untested weights and try again.
				totalWeight -= weights[weightIndex];
			}
		}

		// No block made the cut so return the very last index.
		return Convert.ToInt32(weights[weights.Length - 1]);
	}

	public void InstantiateChunkControllers()
	{
		GameObject lastCreatedObject;
		for (int currentPosX = 0; currentPosX < WorldManager.fillMap.GetUpperBound(0); currentPosX += chunkManagerWidth)
		{
			for (int currentPosY = 0; currentPosY < WorldManager.fillMap.GetUpperBound(1); currentPosY += chunkManagerHeight)
			{
				lastCreatedObject = GameObject.Instantiate(pfab_chunkManager, new Vector3(currentPosX * WorldManager.tileLength, 
														   currentPosY * WorldManager.tileLength * (-1), 0), 
														   Quaternion.identity, transform);
				ChunkController lastCreatedController = lastCreatedObject.GetComponent<ChunkController>();
				lastCreatedController.InitializeVariables(currentPosX, currentPosX + chunkManagerWidth, currentPosY, 
														  currentPosY + chunkManagerHeight);
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
					CreateTile(WorldManager.baseBlock, currentPosX, currentPosY, WorldManager.mineralMap[currentPosX, currentPosY]);
				}
			}
		}
	}

	private GameObject CreateTile(GameObject prefabToInstantiate, int currentPosX, int currentPosY, int blockId)
	{
		GameObject currentTile;
		currentTile = GameObject.Instantiate(prefabToInstantiate, new Vector3(currentPosX * WorldManager.tileLength, 
																			  currentPosY * WorldManager.tileLength * (-1), 0), 
																			  Quaternion.identity, transform);
		currentTile.name = "dirtTile_" + currentPosX.ToString() + "_" + currentPosY.ToString();
		Block currentBlock = (Block)currentTile.GetComponent<IBlock>();
		currentBlock.posX = currentPosX;
		currentBlock.posY = currentPosY;
		currentBlock.FeedBlockData(WorldManager.GetBlockDataOfSpecificId(blockId));
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