using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoker : NPC
{

    [SerializeField] private PathMovement smokerPath;
    [SerializeField] private PathMovement yellingCashierPath;

    [SerializeField] private Vector3 dropCigarettePos;
    [SerializeField] private Vector3 dropCigaretteRot;
    [SerializeField] private Cigarette cig;

    [SerializeField] private ParticleSystem smokerAngry;
    [SerializeField] private ParticleSystem yellingCashierAngry;

    

    protected void OnCollisionEnter(Collision collision) {
        Collider collider = collision.collider;
        if (collider.tag != "Player"){
            UpdateAnger(maxAngerValue);
        }

        base.OnTriggerEnter(collider);
    }

    protected override void IsAngry() {
        if (cig.gameObject.activeSelf) cig.MoveTo(dropCigarettePos, dropCigaretteRot);
        
        if (!smokerPath.Moving) {
            smokerAngry.Play();
            smokerPath.Moving = true;

            yellingCashierAngry.Stop();
            yellingCashierPath.Moving = true;
        }
    }
}
