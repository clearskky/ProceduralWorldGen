using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IPlayer
{    
    [SerializeField] private float raycastDistance;
    [SerializeField] private int movementSpeed;
    [SerializeField] private IInputHandler inputHandler;
    [SerializeField] private Dictionary<int,int> inventory;
    private Rigidbody2D rb;

    //public float maxVelocity;
    public float cash;

    public float baseMiningSpeed;
    public float miningSpeed;
    public int miningSpeedLevel;
    //public float[] miningSpeedLevelModifiers;

    public int baseStorage;
    public int maxStorage;
    public int currentStorage;
    public int storageLevel;
    //public int[] storageLevelModifiers;

    public float baseFuel;
    public float maxFuel;
    public float currentFuel;
    public int fuelTankLevel;
    //public float[] fuelTankLevelModifiers;

    private List<GameObject> listOfInteractables;
    public VignetteController vignetteController;

    public event EventHandler<OnInteractEventArgs> OnInteract;
    public bool playerCanInteract;
    public bool playerCanMove;
    public bool playerCanMine;
    public bool fuelCanDrain;

    public Slider fuelBar;
    public Slider storageBar;
    public Text cashText;

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

    void Start()
    {
        inventory = new Dictionary<int, int>
        {
            {0, 0}
        };        

        cash = PlayerPrefs.GetFloat("cash", 3f);
        AdjustCashText();

        miningSpeed = baseMiningSpeed + UpgradeStation.miningSpeedLevelModifiers[PlayerPrefs.GetInt("miningSpeedLevel", 0)];

        maxStorage = baseStorage + UpgradeStation.storageLevelModifiers[PlayerPrefs.GetInt("inventorySizeLevel", 0)];
        currentStorage = 0;
        AdjustStorageBar();
        //CalculateInventorySize();

        maxFuel = baseFuel + UpgradeStation.fuelTankLevelModifiers[PlayerPrefs.GetInt("fuelTankLevel", 0)];
        currentFuel = maxFuel;

        playerCanInteract = true;
        playerCanMove = true;
        playerCanMine = true;
        listOfInteractables = new List<GameObject>();
        rb = GetComponent<Rigidbody2D>();
        inputHandler = GetComponent<IInputHandler>();
        raycastDistance = gameObject.GetComponent<BoxCollider2D>().bounds.size.x / 2f + 1.4f;
        fuelCanDrain = true;
    }

    void LateUpdate()
    {
        DrainFuel();
    }

    private void DrainFuel()
    {
        if (fuelCanDrain)
        {
            currentFuel -= 0.12f * Time.deltaTime;
            if (currentFuel <= 0)
            {
                InitiateDeathRoutine();
            }
            AdjustFuelBar();
        }
    }

    public IEnumerator MoveTowardsMinedBlock(Vector2 targetPositionToMove, GameObject blockToMine)
    {
        playerCanInteract   = false;
        playerCanMove       = false;
        playerCanMine       = false;

        float distanceDelta = Mathf.Abs(Vector2.Distance(transform.position, targetPositionToMove)) * miningSpeed * Time.deltaTime;

        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;

        while (Vector2.Distance(transform.position, targetPositionToMove) >= 0.1)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPositionToMove, distanceDelta);
            yield return null;
        }
        
        GetComponent<Rigidbody2D>().simulated = true;
        GetComponent<BoxCollider2D>().enabled = true;
        blockToMine.GetComponent<IBlock>().GetMined();
        playerCanInteract   = true;
        playerCanMove       = true;
        playerCanMine       = true;
    }

    public void MineDownwards()
    {
        if (playerCanMine)
        {
            int bitmask = 1 << 8; // Layer we want the raycast to collide with is the Block / 8th Layer
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, raycastDistance, bitmask);

            Vector2 targetPositionToMove = new Vector2(transform.position.x, transform.position.y - WorldManager.tileLength);

            if (hit.collider != null)
            {
                GameObject blockToMine = hit.collider.gameObject;
                StartCoroutine(MoveTowardsMinedBlock(targetPositionToMove, blockToMine));
                //blockToMine.GetComponent<IBlock>().GetMined();
            }
        }
    }
    public void MineLeft()
    {
        if (playerCanMine)
        {
            int bitmask = 1 << 8; // Layer we want the raycast to collide with is the Block / 8th Layer
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.right, raycastDistance, bitmask);

            Vector2 targetPositionToMove = new Vector2(transform.position.x - WorldManager.tileLength, transform.position.y);
            if (hit.collider != null)
            {
                GameObject blockToMine = hit.collider.gameObject;
                StartCoroutine(MoveTowardsMinedBlock(targetPositionToMove, blockToMine));
                //blockToMine.GetComponent<IBlock>().GetMined();
            }
        } 
    }

    public void MineRight()
    {
        if (playerCanMine)
        {
            int bitmask = 1 << 8; // Layer we want the raycast to collide with is the Block / 8th Layer
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, raycastDistance, bitmask);

            Vector2 targetPositionToMove = new Vector2(transform.position.x + WorldManager.tileLength, transform.position.y);

            if (hit.collider != null)
            {
                GameObject blockToMine = hit.collider.gameObject;
                StartCoroutine(MoveTowardsMinedBlock(targetPositionToMove, blockToMine));
                //blockToMine.GetComponent<IBlock>().GetMined();
            }
        }
    }

    public void Interact()
    {
        if (playerCanInteract)
        {
            OnInteract?.Invoke(this, new OnInteractEventArgs() { interactingPlayer = this });   
        }
    }

    private bool DetermineClosestInteractableObject()
    {
        IInteractable closestInteractableObject = null;
        float shortestDistance = 99999; // Just some imposibly large number
        if (listOfInteractables.Count > 0)
        {
            foreach (GameObject interactable in listOfInteractables) // This loop will always give us a closestInteractableObject as long as the list is populated so we don't need to return false in this if block
            {
                if (Vector3.Distance(transform.position, interactable.transform.position) <= shortestDistance) // If the object is closer, assign it to the closest interactable var
                {
                    closestInteractableObject = interactable.GetComponent<IInteractable>();
                }
            }
        }
        else
        {
            Debug.Log("Couldn't determine closest interactable object. Interactable list is not populated.");
            return false;
        }

        OnInteract += closestInteractableObject.OnInteract;
        return true;
    }

    public bool AddObjectToListOfInteractables(GameObject interactable)
    {
        if (DetermineIfInteractableIsInList(interactable) != true)
        {
            listOfInteractables.Add(interactable);
            Debug.Log("Added object to list of interactables");
            return true;
        }
        else
        {
            Debug.Log("Interactable already in list");
            return false;
        }
        
    }

    public bool RemoveObjectFromListOfInteractables(GameObject interactable)
    {
        if (DetermineIfInteractableIsInList(interactable))
        {
            listOfInteractables.Remove(interactable);
            Debug.Log("Removed object from list of interactables");
            return true;
        }
        else
        {
            Debug.Log("Object couldn't be removed from the list of interactables because object wasn't in it.");
            return false;
        }
    }

    private bool DetermineIfInteractableIsInList(GameObject interactable) // Called when player leaves interactable object's trigger
    {
        if (listOfInteractables.Count > 0)
        {
            foreach (var interactableInList in listOfInteractables)
            {
                if (interactableInList.GetComponent<IInteractable>().interactableId == interactable.GetComponent<IInteractable>().interactableId)
                {
                    Debug.Log("Interactable is in list");
                    return true;
                }
                
            }
            Debug.Log("Interactable is not in list");
            return false;
        }
        else
        {
            Debug.Log("Interactable list is not populated");
            return false;
        }
    }


    public void MovePlayerAcordingToInput(Vector2 direction)
    {
        if (direction != Vector2.zero && playerCanMove)
        {
            rb.AddForce(direction * movementSpeed * Time.deltaTime);
            
            //if (rb.velocity.magnitude > maxVelocity)
            //{
            //    rb.AddForce(new Vector2(-rb.velocity.x, -rb.velocity.y) * (rb.velocity.magnitude - maxVelocity) * Time.deltaTime);
            //}
        }
    }

    public void InitiateDeathRoutine()
    {

    }

    public void Refuel(float volumeToRefuel)
    {
        currentFuel += volumeToRefuel;
        Mathf.Clamp(currentFuel, 0, maxFuel);
        AdjustFuelBar();
    }
    public void AdjustFuelBar()
    {
        fuelBar.value = currentFuel / maxFuel;
    }

    public void AdjustStorageBar()
    {
        storageBar.value = (float)currentStorage / (float)maxStorage;
    }

    public void AdjustCash(float changeInCash)
    {
        cash += changeInCash;
        cash = Mathf.Round(cash);
        AdjustCashText();
    }

    public void AdjustCashText()
    {
        cashText.text = "$" + cash.ToString();
    }

    public void AdjustMiningDrillProperties()
    {
        miningSpeed = baseMiningSpeed + UpgradeStation.miningSpeedLevelModifiers[miningSpeedLevel];

        maxStorage = baseStorage + UpgradeStation.storageLevelModifiers[storageLevel];
        AdjustStorageBar();

        maxFuel = baseFuel + UpgradeStation.fuelTankLevelModifiers[fuelTankLevel];
        AdjustFuelBar();
    }

    public void AddBlockToInventory(int blockId, int amount)
    {
        if (currentStorage + amount * WorldManager.blockDatabase[blockId].weight <= maxStorage)
        {
            try
            {
                int currentCount; // currentCount will be zero if the key id doesn't exist
                inventory.TryGetValue(blockId, out currentCount);
                inventory[blockId] = currentCount + amount;
            }
            catch (KeyNotFoundException e)
            {
                inventory.Add(blockId, amount);
            }
            currentStorage += amount * WorldManager.blockDatabase[blockId].weight;
            AudioManager.Instance.PlayMineCompleteClip();
            AdjustStorageBar();
        }
        else
        {
            AudioManager.Instance.PlayMineFailClip();
            Debug.Log("You have no inventory space");
        }
        
    }

    public void CalculateInventorySize()
    {
        int totalWeight = 0;
        foreach (var item in inventory)
        {
            totalWeight += item.Value * WorldManager.blockDatabase[item.Key].weight;
        }
        currentStorage = totalWeight;
        AdjustStorageBar();
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
