using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameCanvasManager : MonoBehaviour, ICanvasManager
{
    private static MainGameCanvasManager _instance;
    public static MainGameCanvasManager Instance
    {
        get { return _instance; }
    }

    public Text dirtCounterText;
    public Text mineralCounterText;
    private int minedMineralCounter, minedDirtCounter;
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
    }
    public void OnBlockMined(object sender, OnBlockMinedEventArgs e)
    {
        switch (e.blockTypeId)
        {
            case 0:
                minedDirtCounter += 1;
                UpdateBrokenDirtBlockCount(minedDirtCounter);
                break;
            case 1:
                minedMineralCounter += 1;
                UpdateBrokenMineralBlockCount(minedMineralCounter);
                break;
        }
    }
    void UpdateBrokenDirtBlockCount(int newCount)
    {
        dirtCounterText.text = newCount.ToString();
    }

    void UpdateBrokenMineralBlockCount(int newCount)
    {
        mineralCounterText.text = newCount.ToString();
    }
}
