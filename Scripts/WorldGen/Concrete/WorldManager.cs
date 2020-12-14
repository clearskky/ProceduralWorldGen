using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

public class WorldManager : MonoBehaviour
{
    public static int width;
    public static int height;
    public static GameObject baseBlock;

    [SerializeField] private int _width, _height;
    [SerializeField] private GameObject _baseBlock;
    

    public static int[,] fillMap;
    public static int[,] mineralMap;
    public static int[,] deltaMap; // Stores the changes player makes to the world

    public static float tileLength;

    public static List<BlockData> blockDatabase;
    public List<BlockData> _blockDatabase;

    private static WorldManager _instance;
    public static WorldManager instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        width = _width;
        height = _height;
        baseBlock = _baseBlock;
        blockDatabase = _blockDatabase;
        tileLength = baseBlock.GetComponent<BoxCollider2D>().size.x;

        if (PlayerPrefs.GetInt("worldHasBeenInitialized", 0) == 1)
        {
            string fillMapJson = PlayerPrefs.GetString("fillMap");
            fillMap = JsonConvert.DeserializeObject<int[,]>(fillMapJson);

            string mineralMapJson = PlayerPrefs.GetString("mineralMap");
            mineralMap = JsonConvert.DeserializeObject<int[,]>(mineralMapJson);
        }
        else
        {
            fillMap = new int[width, height];
            mineralMap = new int[width, height];
        }
        deltaMap = fillMap;
        
        Block.OnBlockMined += Block_OnBlockMined; // Subscribe to the static onblockmined event of the Block class
    }

    private void Block_OnBlockMined(object sender, OnBlockMinedEventArgs blockData)
    {
        deltaMap[blockData.posX, blockData.posY] = 0;
        Debug.Log("You've mined the x:" + blockData.posX + " y:" + blockData.posY + " block brother.");
        SaveGame();
    }

    public void UpdateDeltaMap(int xCoordinate, int yCoordinate)
    {
        deltaMap[xCoordinate, yCoordinate] = 0;
    }

    public static void SaveGame()
    {
        PlayerPrefs.SetString("fillMap", JsonConvert.SerializeObject(deltaMap));
        PlayerPrefs.Save();
    }

    public static BlockData GetBlockDataOfSpecificId(int blockId)
    {
        return blockDatabase.Find(blockData => blockData.blockTypeId.Equals(blockId));
    }
}
