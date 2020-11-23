using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Block Data", menuName = "Block")]
public class BlockData : ScriptableObject
{
    public int blockTypeId;
    public Sprite blockSprite;
    public string vendorName;
    public int weight;
    public int sellValue;
    public bool minable;
}
