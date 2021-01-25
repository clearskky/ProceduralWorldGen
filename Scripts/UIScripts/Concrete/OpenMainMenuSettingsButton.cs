using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMainMenuSettingsButton : MonoBehaviour
{
    public void OpenMainMenuSettings()
    {
        CanvasManager.Instance.EnableSpecificPanel(TogglablePanelType.SettingsPanel);
    }
}
