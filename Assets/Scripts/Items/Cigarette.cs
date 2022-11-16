using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cigarette : Item
{
    [SerializeField] private GameObject futureCig;
    private bool match = false;

    protected override void Update() {
        if (match) MatchCigarettes(futureCig, gameObject);
    }

    public void ThrownToFuture() {
        MatchCigarettes(gameObject, futureCig);
        Dropped();
    }

    public override void PickedUp() {
        StopAllCoroutines();
        base.PickedUp();
    }

    public override void Dropped() {
        StartCoroutine(DropCigarette());
        match = true;
    }

    private IEnumerator DropCigarette() {
        yield return new WaitForSeconds(3f);
        if (!SafePosition()) base.Dropped();
    }

    private bool SafePosition() {
        // At the moment there is no safe place to drop the cigarette
        return false;
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
