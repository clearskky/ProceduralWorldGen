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

    public static event EventHandler<OnBlockMinedEventArgs> OnBlockMined;

    public void FeedBlockData(BlockData blockData) // Block data is fed to the block by the CellularAutomataWorldgen class, this is how it obtains the bare minimum properties
    {
        blockTypeId = blockData.blockTypeId;
        GetComponent<SpriteRenderer>().sprite = blockData.blockSprite;
        minable   = blockData.minable;
    }

    public void GetMined()
    {
        if (minable)
        {            
            OnBlockMined?.Invoke(this, new OnBlockMinedEventArgs() { posX = posX, posY = posY, blockTypeId = blockTypeId });
            Destroy(gameObject);            
        } 
        else
        {
            AudioManager.Instance.PlayInvalidActionClip();
        }
    }
}
