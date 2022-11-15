using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PushPlayer : MonoBehaviour
{
    private float force_backwards = 500f;
    private float force_upwards = 100f;

    private AudioSource ladyVoice;
    private Rigidbody rigidBody;

    [SerializeField] private AudioClip talking1;
    [SerializeField] private AudioClip talking2;
    [SerializeField] private AudioClip talking3;

    [SerializeField] private AudioClip mumbling1;
    [SerializeField] private AudioClip mumbling2;
    [SerializeField] private AudioClip mumbling3;

    public bool mumble = true;  

    void Start() {
        ladyVoice = gameObject.AddComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Player"){ 
            Vector3 force = -(collider.transform.forward * force_backwards) + (collider.transform.up * force_upwards);
            collider.attachedRigidbody.AddForce(force);

            // playAngrySound();
            if (rigidBody) {
                rigidBody.AddForce(transform.up * force_upwards * (rigidBody.mass / collider.attachedRigidbody.mass) / 4);
            }
        }

        if (collider.tag == "Employee"){
            Debug.Log("We should move...");
            transform.position -= transform.right*0.4f; 
        }

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

    private void playAngrySound() {
        AudioClip[] sounds;
        if (mumble) {
            sounds = new AudioClip[] {mumbling1, mumbling2, mumbling3};
        } else {
            sounds = new AudioClip[] {talking1, talking2, talking3};
        }

        System.Random rnd = new System.Random();
        int index = rnd.Next(sounds.Length);
        AudioClip soundToPlay = sounds[index]; 
        if (soundToPlay) {
            ladyVoice.clip = soundToPlay;
            ladyVoice.volume = 1f;
            ladyVoice.Play(0);
        }
        
    }
}
