using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMainMenuButton : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        CanvasManager.Instance.EnableSpecificPanel(TogglablePanelType.MainMenu);
    }
}
