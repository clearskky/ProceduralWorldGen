using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelInteractable : MonoBehaviour, IInteractable
{
    public TogglablePanelType panelToToggle;

    //[field: SerializeField]
    public int interactableId { get; set; }

    void Start()
    {
        interactableId = gameObject.GetInstanceID();
    }

    public void OnInteract(object sender, OnInteractEventArgs playerData)
    {
        Debug.Log("Saving the game");
        WorldManager.SaveGame();
    }

    public void OnTriggerEnter2D(Collider2D actor) // Actor refers to the player object as a whole
    {
        Player playerCharacter = actor.GetComponent<Player>();
        playerCharacter.OnInteract += OnInteract;
        //playerCharacter.AddObjectToListOfInteractables(this.gameObject);

    }

    public void OnTriggerExit2D(Collider2D actor) // Actor refers to the player object as a whole
    {
        Player playerCharacter = actor.GetComponent<Player>();
        playerCharacter.OnInteract -= OnInteract;
        //playerCharacter.RemoveObjectFromListOfInteractables(this.gameObject);
    }
}
