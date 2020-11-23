using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IBlock
{
    public int      posX, posY;
    [SerializeField] private int     blockTypeId;
    [SerializeField] private Sprite  blockSprite;
    [SerializeField] private bool   minable;

    //public BlockData blockData;
    public event EventHandler<OnBlockMinedEventArgs> OnBlockMined;

    public void FeedBlockData(BlockData blockData)
    {
        //this.blockData = blockDataToFeed
        blockTypeId = blockData.blockTypeId;
        GetComponent<SpriteRenderer>().sprite = blockData.blockSprite;
        minable   = blockData.minable;
        OnBlockMined += MainGameCanvasManager.Instance.OnBlockMined;
    }

    public void GetMined()
    {
        if (minable)
        {
            OnBlockMined(this, new OnBlockMinedEventArgs() { posX = posX, posY = posY, blockTypeId = blockTypeId });
            Destroy(gameObject);
        } else
        {
            Debug.Log("Cannot mine this");
        }
    }
}
