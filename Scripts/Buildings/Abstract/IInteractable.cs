using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    int interactableId { get; set; }

    void OnTriggerEnter2D(Collider2D actor);

    void OnTriggerExit2D(Collider2D actor);

    void OnInteract(object sender, OnInteractEventArgs playerData);
}
