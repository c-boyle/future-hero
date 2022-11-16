using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoker : NPC
{

    [SerializeField] private Cigarette cig;
    [SerializeField] private Interactable cigInteract;

    

    protected void OnCollisionEnter(Collision collision) {
        Collider collider = collision.collider;
        if (collider.tag != "Player"){
            UpdateAnger(maxAngerValue);
        }

        base.OnTriggerEnter(collider);
    }

    protected override void IsAngry() {
        if(angry) return;
        angry = true;
        Interactable.UseInteractable(cigInteract, null);
        cig.ThrownToFuture();
    }
}
