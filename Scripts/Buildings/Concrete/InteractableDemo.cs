using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDemo : MonoBehaviour, IInteractable
{
    [field: SerializeField]
    public int interactableId { get; set; }
    public string debugMessage;

    public void OnInteract(object sender, OnInteractEventArgs playerData)
    {
        Debug.LogWarning(debugMessage);
    }

    public void OnTriggerEnter2D(Collider2D actor) // Actor refers to the player object as a whole
    {
        Player playerCharacter = actor.GetComponent<Player>();
        playerCharacter.AddObjectToListOfInteractables(this.gameObject);

    }

    public void OnTriggerExit2D(Collider2D actor) // Actor refers to the player object as a whole
    {
        Player playerCharacter = actor.GetComponent<Player>();
        playerCharacter.RemoveObjectFromListOfInteractables(this.gameObject);
    }
}
