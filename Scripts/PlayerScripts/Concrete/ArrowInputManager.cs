using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowInputManager : MonoBehaviour, IInputHandler
{
    [Range(0,1)]
    public float horizontalMultiplier;
    public void SendMovementInput()
    {
        Player.Instance.MovePlayerAcordingToInput(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
    }
}
