using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IBlock
{
    private int     blockTypeId;
    private string  vendorName;
    private int     sellValue;
    private Sprite  blockSprite;
    private float   toughness;

    public BlockData blockData;
    void Start()
    {
        blockTypeId = blockData.blockTypeId;
        vendorName = blockData.vendorName;
        sellValue = blockData.sellValue;
        GetComponent<SpriteRenderer>().sprite = blockData.blockSprite;
        toughness = blockData.toughness;
    }
}
