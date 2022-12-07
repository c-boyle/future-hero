using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Rigidbody doorRigidBody;
    [SerializeField] private Transform rotateTransform;
    public float degreeChange = 90;
    private float maxAngle = 90;  
    // void OnCollisionStay(Collision collision){
    //     if(doorRigidBody==null)doorRigidBody=GetComponent<Rigidbody>();
    //     if(doorRigidBody) {
    //         doorRigidBody.velocity = Vector3.zero;
    //     }
    // }
    private float orientation = 0;

    void OnCollisionStay(Collision collision) {
        // StopAllCoroutines();
        Debug.Log(collision.collider.tag);
        Vector3 direction = collision.gameObject.transform.position - rotateTransform.position;
        float delta = 0;

        if (Vector3.Dot (rotateTransform.right, direction) > 0) {
            Debug.Log("Left");
            delta = -degreeChange;
        } 
        if (Vector3.Dot (rotateTransform.right, direction) < 0) {
            Debug.Log("Right");
            delta = degreeChange;
        }
        if (Vector3.Dot (rotateTransform.right, direction) == 0) {
            Debug.Log("----");
        }

        Vector3 angles = rotateTransform.rotation.eulerAngles;
        // float change = Mathf.Lerp(0, delta, 0.1f);
        float change = delta;
        if (Mathf.Abs(orientation + change) <= maxAngle) {
            rotateTransform.rotation = Quaternion.Euler(angles + Vector3.up * change);
            orientation += change;
        }
        // StartCoroutine(DoorPush(delta)); 
        
    }

    private IEnumerator DoorPush(float delta) {
        float t = 0.1f;
        var shortWait = new WaitForSeconds(0.0001f);
        Vector3 angles = rotateTransform.rotation.eulerAngles;
        float startOrientation = orientation;
        float changed = 0;
        while(t < 1 && Mathf.Abs(startOrientation + changed) < maxAngle){
            rotateTransform.rotation = Quaternion.Euler(angles + Vector3.up * changed);
            orientation = startOrientation + changed;
            t += 0.01f;
            changed = Mathf.Lerp(0, delta, t);
            yield return shortWait;
        }
    }
}
