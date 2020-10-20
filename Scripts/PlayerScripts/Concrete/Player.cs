using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPlayer
{
    public int movementSpeed;
    
    public IInputHandler inputHandler;

    void Start()
    {
        inputHandler = GetComponent<IInputHandler>();
    }

    private void FixedUpdate()
    {
        MovePlayerAcordingToInput();
    }

    private void MovePlayerAcordingToInput()
    {
        transform.Translate(inputHandler.ReceiveInput() * movementSpeed * Time.deltaTime);
    }
}
