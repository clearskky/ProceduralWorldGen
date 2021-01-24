using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelDrainTrigger : MonoBehaviour, IDrainTrigger
{
    public void OnTriggerEnter2D(Collider2D collider)
    {
        collider.gameObject.GetComponent<Player>().fuelCanDrain = false;
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        collider.gameObject.GetComponent<Player>().fuelCanDrain = true;
    }
}
