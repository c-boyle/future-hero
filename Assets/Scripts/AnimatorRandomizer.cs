using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class AnimatorRandomizer : MonoBehaviour {

    [SerializeField] private Animator animatorToRandomize;

    [SerializeField] private bool randomizeSpeed = false;

    [ConditionalField(nameof(randomizeSpeed))][SerializeField] private float randomSpeedMin; 
    [ConditionalField(nameof(randomizeSpeed))][SerializeField] private float randomSpeedMax;
    
    [SerializeField] private bool randomizeStartTime = false;

    [ConditionalField(nameof(randomizeStartTime))][SerializeField] private float randomStartTimeMin;
    [ConditionalField(nameof(randomizeStartTime))][SerializeField] private float randomStartTimeMax;

    private float savedAnimatorSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if (randomizeSpeed) {
          animatorToRandomize.speed = Random.Range(randomSpeedMin, randomSpeedMax);
        }
        if (randomizeStartTime) {
          savedAnimatorSpeed = animatorToRandomize.speed;
          animatorToRandomize.speed = 0f;
          StartCoroutine(Helpers.InvokeAfterTime(() => animatorToRandomize.speed = savedAnimatorSpeed, Random.Range(randomStartTimeMin, randomStartTimeMax)));
        }
    }
}
