using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{    public void PauseGame()
    {
        Player.Instance.fuelCanDrain = false;
        CanvasManager.Instance.EnableSpecificPanel(TogglablePanelType.PausePanel);
    }
}
