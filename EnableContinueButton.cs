using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableContinueButton : MonoBehaviour
{
    public Button continueButton;
    private void Start()
    {
        if (PlayerPrefs.GetInt("worldHasBeenInitialized", 0) != 1)
        {
            continueButton.enabled = false;
        }
    }
    
}
