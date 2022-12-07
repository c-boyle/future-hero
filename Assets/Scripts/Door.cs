using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Rigidbody doorRigidBody;
    void OnCollisionStay(Collision collision){
        if(doorRigidBody==null)doorRigidBody=GetComponent<Rigidbody>();
        if(doorRigidBody) {
            doorRigidBody.velocity = Vector3.zero;
        }
    }
}
