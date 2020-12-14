using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Continue : MonoBehaviour, IButton
{
    public void OnClick()
    {
        SceneManager.LoadScene(0);
    }
}
