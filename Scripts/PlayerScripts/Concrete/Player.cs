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

    void Update()
    {
        //MovePlayerAcordingToInput();
        MovePlayerAcordingArrowKeys();
    }

    private void MovePlayerAcordingToInput()
    {
        transform.Translate(inputHandler.ReceiveInput() * movementSpeed * Time.deltaTime);
    }
    private void MovePlayerAcordingArrowKeys()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.down * movementSpeed * Time.deltaTime;
        }
    }
}
