using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoker : NPC
{

    [SerializeField] private Cigarette cig;
    [SerializeField] private Interactable cigInteract;

    protected override void IsAngry() {
        if(angry) return;
        angry = true;

        if (cig.held) return;
        
        cig.ThrownToFuture();
        Interactable.UseInteractable(cigInteract, null);
        // cig.RigidBody.isKinematic = false;
        
    }
}
