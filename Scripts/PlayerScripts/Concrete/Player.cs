using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPlayer
{
    private static Player _instance;
    public static Player Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private float raycastDistance;
    public int movementSpeed;
    public IInputHandler inputHandler;

    void Start()
    {
        inputHandler = GetComponent<IInputHandler>();
        raycastDistance = gameObject.GetComponent<BoxCollider2D>().bounds.size.x / 2f + 0.8f;
    }

    void Update()
    {
        Debug.DrawRay(transform.position, -Vector3.up * raycastDistance, Color.green);
        //MovePlayerAcordingToInput();
        MovePlayerAcordingArrowKeys();
        MineDownwards();
    }

    private void MineDownwards()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int bitmask = 1 << 8; // Layer we want the raycast to collide with is the Block / 8th Layer
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, raycastDistance, bitmask);

            if (hit.collider != null)
            {
                hit.collider.gameObject.GetComponent<IBlock>().GetMined();
            }
        }
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
