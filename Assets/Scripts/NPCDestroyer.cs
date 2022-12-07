using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDestroyer : MonoBehaviour
{
    void OnTriggerEnter(Collider collider) {
        if(collider.tag=="Smoker" || collider.tag=="Ruffian") {
            Destroy(collider.gameObject);
        }
    }
}
