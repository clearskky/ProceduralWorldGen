using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_NewGame : MonoBehaviour, IButton
{
    public void OnClick()
    {
        PlayerPrefs.SetInt("worldHasBeenInitialized", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }
}
