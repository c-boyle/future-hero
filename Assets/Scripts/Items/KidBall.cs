using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidBall : Item
{
    private bool firstPickUp = true;
    public override void PickedUp() {
        Rigidbody.isKinematic = false;
        if(firstPickUp) StartCoroutine(PickedUpRoutine());
        firstPickUp = false;
        
    }

    private IEnumerator PickedUpRoutine() {
        yield return new WaitForSeconds(2f);
        base.PickedUp();
    }

    // public override void Dropped() {
    //     match = true;
    //     held = false;
    //     Debug.Log("Drop1");
    // }
}
