using UnityEngine;
using System.Collections;
using System;

public class CellularAutomataWorldgen : MonoBehaviour
{
	public GameObject dirtTile, mineralTile;
	private int tileLength = 75;
	public int width;
	public int height;
	public string seed;

	public int smoothnessIterationCount;

	[Range(0, 100)]
	public int fillPercentage;

	[Range(0, 100)]
	public int mineralPercentage;

	int[,] fillMap;
	int[,] smoothedFillMap;

	int[,] mineralMap;
	int[,] smoothedMineralMap;

	Renderer renderer;

	void Start()
	{
		//if (Input.GetKeyDown(KeyCode.Space))
		//{
			GenerateFillMap();
			GenerateMineralMap();
			//GenerateTexture();
			InstantiateMap();
		//}
	}

	void GenerateFillMap()
	{
		fillMap = new int[width, height];

		System.Random pseudoRandomGenerator = new System.Random(seed.GetHashCode());

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
				{
					fillMap[x, y] = 1; // 1 means the block is filled, 0 means its not.
				}
				else
				{
					fillMap[x, y] = (pseudoRandomGenerator.Next(0, 100) < fillPercentage) ? 1 : 0;
				}
			}
		}

		SmoothFillMap(smoothnessIterationCount);
	}

	void SmoothFillMap(int iterationCount)
	{
		smoothedFillMap = new int[width, height];
		for (int currentIteration = 0; currentIteration < iterationCount; currentIteration++)
		{
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					int neighbourWallTiles = GetSurroundingWallCount(x, y);

					if (neighbourWallTiles > 4)
						smoothedFillMap[x, y] = 1;
					else if (neighbourWallTiles < 4)
						smoothedFillMap[x, y] = 0;

				}
			}
			fillMap = smoothedFillMap;
		}
		
	}

	int GetSurroundingWallCount(int gridX, int gridY)
	{
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
		{
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
			{
				if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
				{
					if (neighbourX != gridX || neighbourY != gridY)
					{
						wallCount += fillMap[neighbourX, neighbourY];
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
		mineralMap = new int[width, height];

		System.Random pseudoRandomGenerator = new System.Random(seed.GetHashCode());

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
				{
					mineralMap[x, y] = 0; // 1 means the block is a mineral block, 0 means its not.
				}
				else if (fillMap[x,y] == 1)
				{
					mineralMap[x, y] = (pseudoRandomGenerator.Next(0, 100) < mineralPercentage) ? 1 : 0;
				}
				else
				{
					mineralMap[x, y] = 0;
				}
			}
		}
	}

	public void GenerateTexture()
	{
		renderer = GetComponent<Renderer>();
		Texture2D texture = new Texture2D(width, height);

		for (int currentPosX = 0; currentPosX < width; currentPosX++)
		{
			for (int currentPosY = 0; currentPosY < height; currentPosY++)
			{
				Color color = (fillMap[currentPosX, currentPosY] == 1) ? new Color(0,0,0) : new Color(255,255,255);
				if (color.r <= 0.0f) // Only way the red channel can be zero is if its a filled square.
				{
					color = (mineralMap[currentPosX, currentPosY] == 1) ? new Color(0, 255, 0) : new Color(0, 0, 0);
				}
				texture.SetPixel(currentPosX, currentPosY, color);
			}
		}
		texture.filterMode = FilterMode.Point;
		texture.Apply();
		renderer.material.mainTexture = texture;
	}

	public void InstantiateMap()
	{
		for (int currentPosX = 0; currentPosX < fillMap.GetUpperBound(0); currentPosX++)
		{
			for (int currentPosY = 0; currentPosY < fillMap.GetUpperBound(1); currentPosY++)
			{
				if (fillMap[currentPosX, currentPosY] == 1)
				{
					if (mineralMap[currentPosX, currentPosY] == 1)
					{
						GameObject.Instantiate(mineralTile, new Vector3(currentPosX * tileLength, currentPosY * tileLength, 10), Quaternion.identity);
					}
					else
					{
						GameObject.Instantiate(dirtTile, new Vector3(currentPosX * tileLength, currentPosY * tileLength, 10), Quaternion.identity);
					}
				}
			}
		}
	}
}