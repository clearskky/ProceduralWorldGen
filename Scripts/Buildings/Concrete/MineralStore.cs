using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineralStore : MonoBehaviour, IInteractable, IBuilding
{
    public int interactableId { get; set; }
    private Player interactingPlayer;

    public Text totalRevenueText;
    private int totalInteractingPlayerRevenue;

    void Start()
    {
        interactableId = gameObject.GetInstanceID();
    }

    public void OnTriggerEnter2D(Collider2D actor)
    {
        interactingPlayer = actor.GetComponent<Player>();
        interactingPlayer.OnInteract += OnInteract;
    }

    public void OnTriggerExit2D(Collider2D actor)
    {
        interactingPlayer = actor.GetComponent<Player>();
        interactingPlayer.OnInteract -= OnInteract;
    }

    public void OnInteract(object sender, OnInteractEventArgs playerData)
    {
        CanvasManager.Instance.EnableSpecificPanel(TogglablePanelType.MineralVendor);
        interactingPlayer.fuelCanDrain = false;
        AdjustUIElements();
    }

    private void AdjustUIElements()
    {
        CalculatePlayerRevenue();
        totalRevenueText.text = "$" + totalInteractingPlayerRevenue.ToString();
    }

    public void CalculatePlayerRevenue()
    {
        totalInteractingPlayerRevenue = 0;
        foreach (var mineral in interactingPlayer.inventory)
        {
            totalInteractingPlayerRevenue += WorldManager.blockDatabase[mineral.Key].sellValue * mineral.Value; //Key is the mineral id, value is how many instances are in the inventory
        }
    }

    public void SellInteractingPlayerMinerals()
    {
        totalInteractingPlayerRevenue = 0;
        //foreach (var mineral in interactingPlayer.inventory)
        //{
        //    totalInteractingPlayerRevenue += WorldManager.blockDatabase[mineral.Key].sellValue * mineral.Value; //Key is the mineral id, value is how many instances are in the inventory
        //    interactingPlayer.inventory.Remove(mineral.Key);
        //}
        List<int> keys = new List<int>(interactingPlayer.inventory.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            totalInteractingPlayerRevenue += WorldManager.blockDatabase[keys[i]].sellValue * interactingPlayer.inventory[keys[i]]; //Key is the mineral id, value is how many instances are in the inventory
            interactingPlayer.inventory[keys[i]] = 0;
        }        
        interactingPlayer.CalculateInventorySize();
        interactingPlayer.AdjustCash(totalInteractingPlayerRevenue);
        interactingPlayer.fuelCanDrain = true;
        AudioManager.Instance.PlaySellInventoryClip();
        CanvasManager.Instance.EnableSpecificPanel(TogglablePanelType.PlayerInterface);
    }    
}
