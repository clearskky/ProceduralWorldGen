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
    public CellularAutomataWorldgen worldGen;

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

        Block.OnBlockMined += Block_OnBlockMined; // Subscribe to the static onblockmined event of the Block class
        worldGen = GetComponent<CellularAutomataWorldgen>();
        width = _width;
        height = _height;
        baseBlock = _baseBlock;
        blockDatabase = _blockDatabase;
        tileLength = baseBlock.GetComponent<BoxCollider2D>().size.x;

        if (PlayerPrefs.GetInt("worldHasBeenInitialized", 0) == 1)
        {
            Debug.Log("World has already been initialized, pulling data from playerprefs.");

            string fillMapJson = PlayerPrefs.GetString("fillMap");
            fillMap = JsonConvert.DeserializeObject<int[,]>(fillMapJson);
            Debug.Log("Fill map obtained: " + PlayerPrefs.GetString("fillMap"));

            string mineralMapJson = PlayerPrefs.GetString("mineralMap");
            mineralMap = JsonConvert.DeserializeObject<int[,]>(mineralMapJson);
            Debug.Log("Mineral map obtained." + PlayerPrefs.GetString("mineralMap"));
        }
        else
        {
            Debug.Log("Initializing world for the first time.");
            fillMap = new int[width, height];
            mineralMap = new int[width, height];
            worldGen.InitializeWorld();
        }

        deltaMap = fillMap;

        Debug.Log("Instantiating World");
        worldGen.InstantiateWorld();

        Block.OnBlockMined += Block_OnBlockMined; // Subscribe to the static onblockmined event of the Block class
    }

    private void Block_OnBlockMined(object sender, OnBlockMinedEventArgs blockData)
    {
        deltaMap[blockData.posX, blockData.posY] = 0;
        Debug.Log("You've mined the x:" + blockData.posX + " y:" + blockData.posY + " block");
        //SaveGame();
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
