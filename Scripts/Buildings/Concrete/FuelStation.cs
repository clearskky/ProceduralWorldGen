using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelStation : MonoBehaviour, IBuilding, IInteractable
{
    public int interactableId { get; set; }
    private Player interactingPlayer;
    public float costPerLiter;

    public Text refuelAmountText;
    public Text refuelCostText;
    public Text cashText;

    void Start()
    {
        costPerLiter = 0.8f;
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
        CanvasManager.Instance.EnableSpecificPanel(TogglablePanelType.FuelVendor);
        interactingPlayer.fuelCanDrain = false;
        AdjustUIElements();
    }

    public void AdjustUIElements()
    {
        float missingFuel = interactingPlayer.maxFuel - interactingPlayer.currentFuel;
        float refuelingCost;
        float volumeToRefuel;
        if (missingFuel * costPerLiter > interactingPlayer.cash)
        {
            refuelingCost = interactingPlayer.cash;
            volumeToRefuel = refuelingCost / costPerLiter;
        }
        else
        {
           volumeToRefuel = interactingPlayer.maxFuel - interactingPlayer.currentFuel;
           refuelingCost = volumeToRefuel * costPerLiter;
        }
        refuelAmountText.text = volumeToRefuel.ToString();
        refuelCostText.text = refuelingCost.ToString();
        cashText.text = interactingPlayer.cash.ToString();
    }

    public void RefuelTheInteractingPlayer()
    {
        float refuelingCost;
        float volumeToRefuel;
        float missingFuel = interactingPlayer.maxFuel - interactingPlayer.currentFuel;
        if (missingFuel * costPerLiter > interactingPlayer.cash)
        {
            refuelingCost = interactingPlayer.cash;
            volumeToRefuel = refuelingCost / costPerLiter;
            interactingPlayer.AdjustCash(refuelingCost * -1.0f);
            interactingPlayer.Refuel(volumeToRefuel);
        }
        else
        {
            volumeToRefuel = interactingPlayer.maxFuel - interactingPlayer.currentFuel;
            refuelingCost = volumeToRefuel * costPerLiter;
            interactingPlayer.AdjustCash(refuelingCost * -1);
            interactingPlayer.Refuel(volumeToRefuel);
        }
        interactingPlayer.fuelCanDrain = true;
        CanvasManager.Instance.EnableSpecificPanel(TogglablePanelType.PlayerInterface);
    }
}
