using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : Item
{
    private bool triggered = false;
    [SerializeField] private NPC owner; 

    // void OnCollisionEnter(Collision collision) { <--- Possible alternative of triggering owner
    protected override void MovementHappening() {
        if(!triggered && LevelTimer.SecondsSpentInLevel > 2f ) {    
            OwnerIsAngry();
            triggered = true;
        }
        base.MovementHappening();
    }

    public void OwnerIsAngry() {
        if (owner != null) owner.SetAngry();
    }
}
