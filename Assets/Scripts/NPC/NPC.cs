using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPC : MonoBehaviour
{
    protected int angerLevel = 0;
    protected int maxAngerValue = 10;
    protected bool angry = false;

    private float force_backwards = 500f;
    private float force_upwards = 100f;

    private AudioSource voice;
    private Rigidbody rigidBody;

    [SerializeField] private AudioClip talking1;
    [SerializeField] private AudioClip talking2;
    [SerializeField] private AudioClip talking3;

    [SerializeField] private AudioClip mumbling1;
    [SerializeField] private AudioClip mumbling2;
    [SerializeField] private AudioClip mumbling3;

    public bool mumble = true;  

    void Start() {
        voice = gameObject.AddComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody>();
    }

    protected virtual void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Player"){ 
            Vector3 force = -(collider.transform.forward * force_backwards) + (collider.transform.up * force_upwards);
            collider.attachedRigidbody.AddForce(force);

            // // playAngrySound();
            // if (rigidBody) {
            //     rigidBody.AddForce(transform.up * force_upwards * (rigidBody.mass / collider.attachedRigidbody.mass) / 4);
            // }

            UpdateAnger(angerLevel+1);
        }

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
            voice.clip = soundToPlay;
            voice.volume = 1f;
            voice.Play(0);
        }
        
    }

    public void UpdateAnger(int anger) {
        Debug.Log("we are now " + anger + " angry!");
        angerLevel = anger;
        if (angerLevel >= maxAngerValue) {
            IsAngry();
        }
    }

    protected virtual void IsAngry(){
        Debug.Log("You wouldn't like me when I'm angry... >:(");
    }

}
