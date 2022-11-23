using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cigarette : Item
{
    [SerializeField] private GameObject futureCig;
    private bool match = false;
    [SerializeField] private ParticleSystem smoke;
    public bool smoking = true;
    private bool danger = false;

    protected override void Update() {
        if (match) MatchCigarettes(futureCig, gameObject);
    }

    void OnCollisionEnter(Collision collision) {
        CheckCig(collision.gameObject.GetComponent<Interactable>());
    }

    void OnTriggerEnter(Collider collider) {
        CheckCig(collider.gameObject.GetComponent<Interactable>()); 
    }

    private void CheckCig(Interactable interactable) {
        if (!smoking || !match) return; // If cigarette is not smoking, no risk. Return
        
        if (interactable != null) { // If we are given a suitable interactable, it can put cig out
            SafePosition();

            Cup cup = interactable.gameObject.GetComponent<Cup>(); // If it's a cup, owner is mad
            if (cup != null) cup.OwnerIsAngry();
        }
        else if (!held && !danger) {
            base.Dropped(); // No interactable, so fire happens!
            danger = true;
        }
    }

    public void ThrownToFuture() {
        if (held) return;
        MatchCigarettes(gameObject, futureCig);
    }

    public override void PickedUp() {
        match = true;
        if (smoking) base.PickedUp();
        else held = true;
        danger = false;
        
    }

    public override void Dropped() {
        match = true;
        held = false;
        Debug.Log("Drop1");
    }

    public void SafePosition() {
        base.PickedUp();
        PutOut();
    }

    private void PutOut() {
        smoking = false;
        if (smoke != null) smoke.Stop();
    }

    private void MatchCigarettes(GameObject cig1, GameObject cig2) {
        Transform toBe = cig2.transform;
        Transform isNow = cig1.transform;

        Vector3 originalScale = isNow.lossyScale;
        Vector3 futureScale = toBe.lossyScale;
        Vector3 itemScale = isNow.localScale;

        // Rigidbody.MovePosition(future.position);
        // Rigidbody.MoveRotation(future.rotation);
        cig1.transform.position = toBe.position;
        cig1.transform.rotation = toBe.rotation;
        
        // Revert the size of cig to what it should be in future --> SHOULD TURN TO Item FUNCTION time permitting!
        cig1.transform.localScale = new(itemScale.x * futureScale.x / originalScale.x, 
                               itemScale.y * futureScale.y / originalScale.y, 
                               itemScale.z * futureScale.z / originalScale.z);
    }
}
