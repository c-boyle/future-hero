using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingCashier : NPC
{
    protected override void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Employee"){
            Debug.Log("We should move...");
            transform.position -= transform.right*0.4f; 
        }

        base.OnTriggerEnter(collider);
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.tag == "Employee"){
            StartCoroutine(WaitABit());
        }
    }

    private IEnumerator WaitABit() {
        yield return new WaitForSeconds(2f);
        transform.position += transform.right*0.4f;  
    }
}
