using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputHandler : MonoBehaviour, IInputHandler
{
    public Joystick joystick;
    private Touch theTouch;
    private Vector2 lastKnownDirection;

    public Vector2 ReceiveInput()
    {
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);
            if (theTouch.phase == TouchPhase.Moved)
            {
                lastKnownDirection = joystick.Direction;
            }
            else if (theTouch.phase == TouchPhase.Stationary)
            {
                lastKnownDirection = joystick.Direction;
            }
        }

        return lastKnownDirection;
    }
}
