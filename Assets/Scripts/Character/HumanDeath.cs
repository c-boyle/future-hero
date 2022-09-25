using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanDeath : MonoBehaviour
{
    public AudioSource deathSound;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Train")
        {
            deathSound.Play(0);
            Destroy(this.gameObject);
        }
    }
}
