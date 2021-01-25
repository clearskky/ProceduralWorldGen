using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitToMainMenuButton : MonoBehaviour
{
    public void QuitToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1); // Main Menu
    }
}
