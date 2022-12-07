using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDestroyer : MonoBehaviour
{
    void OnTriggerEnter(Collider collider) {
        Debug.Log("Kalb is " + collider.tag);
        if(collider.tag=="Smoker" || collider.tag=="Ruffian") {
            Destroy(collider.gameObject);
        }
    }
}
