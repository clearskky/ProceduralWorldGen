using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPlayerInterfaceButton : MonoBehaviour
{
    public void ReturnToPlayerInterface()
    {
        Player.Instance.fuelCanDrain = true;
        CanvasManager.Instance.EnableSpecificPanel(TogglablePanelType.PlayerInterface);
    }
}
