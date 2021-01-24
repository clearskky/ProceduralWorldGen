using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIPanel : MonoBehaviour, IUIPanel
{
    public TogglablePanelType togglablePanelType;
    public TogglablePanelType previousPanelType;
}