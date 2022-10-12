using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Add randomness
// TODO: Add horizontal bob tilting
// TODO: Make fps arms drift behinid offset
// TODO: Animate bob to a stop when player stops moving
public class CameraBob : MonoBehaviour {
    [SerializeField] private Camera cam;
    [SerializeField] private float breathIntensity = 1;
    [SerializeField] private float breathSpeed = 1;
    [SerializeField] private float bobIntensity = 1;
    [SerializeField] private float bobSpeed = 1;

    private float camInitialLocalY;
    private int breathFrameCounter = 0;
    private int bobFrameCounter = 0;
    public bool isBobing = true;
    public float currentOffset = 0;


    void OnValidate() {
        breathIntensity = Mathf.Max(0, breathIntensity);
        breathSpeed = Mathf.Max(0, breathSpeed);
        bobIntensity = Mathf.Max(0, bobIntensity);
        bobSpeed = Mathf.Max(0, bobSpeed);
    }

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
        currentOffset = calculateBreathOffset() + calculateBobOffset();
        cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, camInitialLocalY + currentOffset, cam.transform.localPosition.z);

        if (isBobing) bobFrameCounter += 1;
        breathFrameCounter += 1;
    }
}
