using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private static BlockManager _instance;
    public static BlockManager Instance { get { return _instance; } }

    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(_instance);
        }
        else
        {
            _instance = this;
        }
    }

    public void OnBlockDestroyed(OnBlockMinedEventArgs eventArgs)
    {
        switch (eventArgs.breakerSource)
        {
            case BreakerSources.Drilling:
                Player.Instance.AddBrokenBlockToInventory();
                break;
            case BreakerSources.Powerup:
                break;
            case BreakerSources.WorldEvent:
                break;
            default:
                break;
        }
    }
}
