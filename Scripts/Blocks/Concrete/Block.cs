using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IBlock
{
    public int      posX, posY;
    private int     blockTypeId;
    private string  vendorName;
    private int     sellValue;
    private Sprite  blockSprite;
    private float   toughness;

    public BlockData blockData;
    public event EventHandler<OnBlockMinedEventArgs> OnBlockMined;
    void Start()
    {
        blockTypeId = blockData.blockTypeId;
        vendorName  = blockData.vendorName;
        sellValue   = blockData.sellValue;
        GetComponent<SpriteRenderer>().sprite = blockData.blockSprite;
        toughness   = blockData.toughness;
        OnBlockMined += MainGameCanvasManager.Instance.OnBlockMined;
    }

   

    public void GetMined()
    {
        OnBlockMined(this, new OnBlockMinedEventArgs() { posX = posX, posY = posY, blockTypeId = blockTypeId });
        Destroy(gameObject);
    }
}
