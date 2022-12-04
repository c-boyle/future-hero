using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingCashier : NPC
{
    [SerializeField] private Transform nextPos;
    private bool moved = false;
    
    protected override void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Employee" && !moved){
            Debug.Log("We should move...");
            // transform.position -= transform.right*0.4f; 
            MoveOutOfWay(true);
        }

        base.OnTriggerEnter(collider);
    }

    // private void OnTriggerExit(Collider collider) {
    //     if (collider.tag == "Employee" && moved){
    //         StartCoroutine(WaitABit());
    //     }
    // }

    private void MoveOutOfWay(bool outOfWay){
        Debug.Log("Moving outOfWay?" + outOfWay);
        if(outOfWay) { 
            MatchTransform(nextPos);
            StartCoroutine(WaitABit());
        } else {
            MatchTransform(nextPos);
        }

        moved = outOfWay;
    }

    private void MatchTransform(Transform toMatch) {
        Vector3 tmpPos = transform.position;
        Quaternion tmpRot = transform.rotation;

        transform.position = toMatch.position;
        transform.rotation = toMatch.rotation;

        toMatch.position = tmpPos;
        toMatch.rotation = tmpRot;
    }

    private IEnumerator WaitABit() {
        yield return new WaitForSeconds(2f);
        // transform.position += transform.right*0.4f;  
        MoveOutOfWay(false);
    }
}
