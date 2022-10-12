using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

// TODO: Add randomness
// TODO: Add horizontal bob tilting
// TODO: Increase bob intensity when sprinting
// TODO: Make fps arms drift behind offset
// TODO: Animate bob to a stop when player stops moving
public class CameraBob : MonoBehaviour {
    [SerializeField] [MustBeAssigned] private Camera cam;
    [SerializeField] [PositiveValueOnly] private float breathIntensity = 1;
    [SerializeField] [PositiveValueOnly] private float breathSpeed = 1;
    [SerializeField] [PositiveValueOnly] private float bobIntensity = 1;
    [SerializeField] [PositiveValueOnly] private float bobSpeed = 1;

    private float camInitialLocalY;
    private int breathFrameCounter = 0;
    private int bobFrameCounter = 0;
    public bool isEnabled = true;
    private bool isBreathing = true;
    public bool isBobing = false;
    [ReadOnly] public float currentOffset = 0;

    void Start() {
        camInitialLocalY = cam.transform.localPosition.y;
    }

    private float calculateBreathOffset() {
        float speed = breathSpeed * 0.002f;
        float strength = breathIntensity * 0.002f;
        return Mathf.Sin(breathFrameCounter * speed) * strength;
    }

    private float calculateBobOffset() {
        float speed = bobSpeed * 0.007f;
        float strength = bobIntensity * 0.005f;
        return -Mathf.Abs(Mathf.Sin(bobFrameCounter * speed) * strength);
    }

    void Update() {
        if (isEnabled) {
            currentOffset = calculateBreathOffset() + calculateBobOffset();
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, camInitialLocalY + currentOffset, cam.transform.localPosition.z);

            if (isBreathing) breathFrameCounter += 1;
            if (isBobing) bobFrameCounter += 1;
        }
    }
}
