using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchInputHandler : MonoBehaviour, IInputHandler
{
    public Joystick joystick;
    private Touch theTouch;
    public Text phaseDisplayText;

    void Update()
    {
        SendMovementInput();
    }

    public void SendMovementInput()
    {
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);
            if (theTouch.phase == TouchPhase.Moved)
            {
                Player.Instance.MovePlayerAcordingToInput(joystick.Direction);
            }
            else if (theTouch.phase == TouchPhase.Stationary)
            {
                Player.Instance.MovePlayerAcordingToInput(joystick.Direction);
            }
        }
    }
}
