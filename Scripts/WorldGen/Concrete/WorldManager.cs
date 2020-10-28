using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static int width;
    public static int height;

    [SerializeField] private int _width, _height;

    public static int[,] fillMap;
    public static int[,] mineralMap;

    public static GameObject dirtTile;
    public static GameObject mineralTile;
    public static float tileLength;

    [SerializeField] private GameObject _dirtTile, _mineralTile;

    void Awake()
    {
        width = _width;
        height = _height;

        dirtTile = _dirtTile;
        mineralTile = _mineralTile;
        tileLength = dirtTile.GetComponent<BoxCollider2D>().size.x;
    }
}
