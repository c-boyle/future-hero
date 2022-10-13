using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

// TODO: Add randomness
// TODO: Add horizontal bob tilting
// TODO: Increase bob intensity when sprinting
// TODO: Make fps arms drift behind offset
// TODO: Stop bobbing when walking into a wall
public class CameraBob : MonoBehaviour {
    [SerializeField] [MustBeAssigned] private Camera cam;
    [SerializeField] [PositiveValueOnly] private float breatheIntensity = 1;
    [SerializeField] [PositiveValueOnly] private float breatheSpeed = 1;
    [SerializeField] [PositiveValueOnly] private float bobIntensity = 1;
    [SerializeField] [PositiveValueOnly] private float bobSpeed = 1;
    private float _breatheIntensity;
    private float _breatheSpeed;
    private float _bobIntensity;
    private float _bobSpeed;

    private float camInitialLocalY;
    private float breatheFrameCounter = 0;
    private float bobFrameCounter = 0;
    private float breatheOffset = 0;
    private float bobOffset = 0;
    [ReadOnly] public float offset = 0;

    public bool isEnabled = true;
    public bool isBreathing = true;
    public bool isBobbing = false;

    void Start() {
        camInitialLocalY = cam.transform.localPosition.y;
        _breatheIntensity = breatheIntensity * 0.001f;
        _breatheSpeed = breatheSpeed * 0.002f;
        _bobIntensity = bobIntensity * 0.002f;
        _bobSpeed = bobSpeed * 0.009f;
    }

    private float calculateBreatheOffset(float x) {
        return Mathf.Sin(x) * _breatheIntensity;
    }

    private float calculateBobOffset(float x) {
        return -Mathf.Abs(Mathf.Sin(x)) * _bobIntensity;
    }

    private float getDistanceToPeriodEnds(float x) {
        float periodLength = Mathf.PI;
        float distToPeriodBeginning = x % periodLength;
        float distToPeriodEnd = periodLength - (x % periodLength);

        if (distToPeriodBeginning < distToPeriodEnd) {
            // x is closer to beginning of period than end, return negative distance
            if (distToPeriodBeginning >= 0.001) return -distToPeriodBeginning;
        }
        else if (distToPeriodBeginning >= distToPeriodEnd) {
            // x is closer to end of period than beginning, return positive distance
            if (distToPeriodEnd >= 0.001) return distToPeriodEnd;
        }

        return 0;
    }

    void Update() {
        if (isEnabled) {
            breatheOffset = calculateBreatheOffset(breatheFrameCounter);
            bobOffset = calculateBobOffset(bobFrameCounter);
            offset = breatheOffset + bobOffset;
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, camInitialLocalY + offset, cam.transform.localPosition.z);

            if (isBreathing) {
                breatheFrameCounter += 1 * _breatheSpeed;
            }
            else if (!isBreathing) {
                breatheFrameCounter += Mathf.Clamp(getDistanceToPeriodEnds(breatheFrameCounter), -0.5f, 0.5f) * _breatheSpeed;
            }

            if (isBobbing) {
                bobFrameCounter += 1 * _bobSpeed;
            }
            else if (!isBobbing) {
                bobFrameCounter += Mathf.Clamp(getDistanceToPeriodEnds(bobFrameCounter), -0.5f, 0.5f) * _bobSpeed;
            }
        }
    }
}
