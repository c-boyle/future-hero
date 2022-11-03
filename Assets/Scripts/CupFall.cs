using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{
    // [SerializeField]
    // Start is called before the first frame update
    public void playAnimation(){ 
        GetComponent<Animation>().Play();
    }
}
