using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OldLady : MonoBehaviour
{
    private float force_backwards = 500f;
    private float force_upwards = 200f;

    [SerializeField] AudioSource ladyTalking1; 
    [SerializeField] AudioSource ladyTalking2;
    [SerializeField] AudioSource ladyTalking3;

    [SerializeField] AudioSource ladyMumbling1; 
    [SerializeField] AudioSource ladyMumbling2; 
    [SerializeField] AudioSource ladyMumbling3; 

    public bool mumble = true;  

    private void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Player"){ 
            Debug.Log("YAY");
            Vector3 force = -(collider.transform.forward * force_backwards) + (collider.transform.up * force_upwards);
            collider.attachedRigidbody.AddForce(force);

            playAngrySound();
        }
    }

    private void playAngrySound() {
        AudioSource[] sounds;
        if (mumble) {
            sounds = new AudioSource[] {ladyMumbling1, ladyMumbling2, ladyMumbling3};
        } else {
            sounds = new AudioSource[] {ladyTalking1, ladyTalking2, ladyTalking3};
        }

        System.Random rnd = new System.Random();
        int index = rnd.Next(sounds.Length);
        AudioSource soundToPlay = sounds[index]; 
        if (soundToPlay) {
            sounds[index].Play();
        }
        
    }
}
