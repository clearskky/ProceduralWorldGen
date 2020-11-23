using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldManager : MonoBehaviour
{
    public static int width;
    public static int height;
    public static GameObject baseBlock;

    [SerializeField] private int _width, _height;
    [SerializeField] private GameObject _baseBlock;

    public static int[,] fillMap;
    public static int[,] mineralMap;

    public static float tileLength;

    public static List<BlockData> blockDatabase;
    public List<BlockData> _blockDatabase;

    void Awake()
    {
        width = _width;
        height = _height;
        baseBlock = _baseBlock;
        blockDatabase = _blockDatabase;

        //dirtTile = _dirtTile;
        //mineralTile = _mineralTile;
        tileLength = baseBlock.GetComponent<BoxCollider2D>().size.x;
    }



    public static BlockData GetBlockDataOfSpecificId(int blockId)
    {
        return blockDatabase.Find(blockData => blockData.blockTypeId.Equals(blockId));
    }

    /*
    public static IDictionary<int, MineralInventoryData> blockData = new Dictionary<int, MineralInventoryData>()
    {
        {1, new MineralInventoryData{vendorName = "Bronze", weight = 1, sellValue = 100}},
        {2, new MineralInventoryData{vendorName = "Iron", weight = 1, sellValue = 200}},
        {3, new MineralInventoryData{vendorName = "Silver", weight = 1, sellValue = 500}},
        {4, new MineralInventoryData{vendorName = "Gold", weight = 1, sellValue = 1000}}
    };
    */
}
