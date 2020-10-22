using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Block Data", menuName = "Block")]
public class BlockData : ScriptableObject
{
    public int blockTypeId;
    public string vendorName;
    public int sellValue;
    public Sprite blockSprite;
    public float toughness;
}
