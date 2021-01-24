using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDrainTrigger
{
    void OnTriggerEnter2D(Collider2D collider);
    void OnTriggerExit2D(Collider2D collider);
}
