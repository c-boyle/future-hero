using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

// TODO: Add randomness
// TODO: Add rotational inertia
// TODO: add jump drift

public class ViewBob : MonoBehaviour {
    [SerializeField] [MustBeAssigned] private CharacterMovement movement;
    [SerializeField] [MustBeAssigned] private Transform armsTransform;
    [SerializeField] [MustBeAssigned] private Transform cameraTransform;

    [SerializeField] [PositiveValueOnly] private float breatheIntensity = 1;
    [SerializeField] [PositiveValueOnly] private float breatheSpeed = 1;
    [SerializeField] [PositiveValueOnly] private float bobIntensity = 1;
    [SerializeField] [PositiveValueOnly] private float bobSpeed = 1;
    private float dragTransition = 0.05f; // how fast the arm pulls back when you begin moving
    private float dragMax = -0.06f; // how much the arm pulls back when you begin moving

    [SerializeField] [ReadOnly] private float dragOffset = 0;
    [SerializeField] [ReadOnly] private float bobVerticalOffset = 0;
    [SerializeField] [ReadOnly] private float bobLeftOffset = 0;
    [SerializeField] [ReadOnly] private float bobRightOffset = 0;
    [SerializeField] [ReadOnly] private float breatheOffset = 0;
    private Coroutine currentBobVerticalRoutine;
    private Coroutine currentBobLeftRoutine;
    private Coroutine currentBobRightRoutine;
    private Coroutine currentBreatheRoutine;

    private Vector3 cameraInitialLocalPosition;
    private Vector3 armsInitialLocalPosition;
    public bool isEnabled = true;
    public float globalSpeedMultiplier; // bob speed multiplier for sprinting

    // More constants
    private const float BOB_VERTICAL_INTENSITY_MULTIPLIER = 0.02f;
    private const float BOB_VERTICAL_SPEED_MULTIPLIER = 7f;
    private const float BOB_LEFT_INTENSITY_MULTIPLIER = 0.02f;
    private const float BOB_LEFT_SPEED_MULTIPLIER = 7f;
    private const float BOB_RIGHT_INTENSITY_MULTIPLIER = 0.02f;
    private const float BOB_RIGHT_SPEED_MULTIPLIER = 7f;
    private const float BREATHE_INTENSITY_MULTIPLIER = 0.008f;
    private const float BREATHE_SPEED_MULTIPLIER = 2f;
    private const float DRAG_Z_MULTIPLIER = 1f;
    private const float DRAG_Y_MULTIPLIER = 0.25f;
    private Vector3 CAMERA_SCALE_BREATHE = new Vector3(0, 0.08f, 0); // percentage of the breathe offset that the camera uses
    private Vector3 CAMERA_SCALE_BOB = new Vector3(0, 0.1f, 0);

    void Start() {
        cameraInitialLocalPosition = cameraTransform.localPosition;
        armsInitialLocalPosition = armsTransform.localPosition;
    }

    IEnumerator BobVertical() {
        float INTENSITY = bobIntensity * BOB_VERTICAL_INTENSITY_MULTIPLIER;
        float SPEED = bobSpeed * BOB_VERTICAL_SPEED_MULTIPLIER * globalSpeedMultiplier;

        float x = 0;
        while (x <= Mathf.PI) {
            bobVerticalOffset = -Mathf.Abs(Mathf.Sin(x)) * INTENSITY;

            yield return null; //Waits/skips one frame

            x += Time.deltaTime * SPEED;
        }

        bobVerticalOffset = 0;
        currentBobVerticalRoutine = null;
    }

    IEnumerator BobLeft() {
        float INTENSITY = bobIntensity * BOB_LEFT_INTENSITY_MULTIPLIER;
        float SPEED = bobSpeed * BOB_LEFT_SPEED_MULTIPLIER * globalSpeedMultiplier;

        float x = 0;
        while (x <= Mathf.PI) {
            bobLeftOffset = -Mathf.Abs(Mathf.Sin(x)) * INTENSITY;

            yield return null; //Waits/skips one frame

            x += Time.deltaTime * SPEED;
        }

        bobLeftOffset = 0;
        currentBobLeftRoutine = null;
    }

    IEnumerator BobRight() {
        float INTENSITY = bobIntensity * BOB_RIGHT_INTENSITY_MULTIPLIER * -1; // negative sign here
        float SPEED = bobSpeed * BOB_RIGHT_SPEED_MULTIPLIER * globalSpeedMultiplier;

        float x = 0;
        while (x <= Mathf.PI) {
            bobRightOffset = -Mathf.Abs(Mathf.Sin(x)) * INTENSITY;

            yield return null; //Waits/skips one frame

            x += Time.deltaTime * SPEED;
        }

        bobRightOffset = 0;
        currentBobRightRoutine = null;
    }

    IEnumerator Breathe() {
        float INTENSITY = breatheIntensity * BREATHE_INTENSITY_MULTIPLIER;
        float SPEED = breatheSpeed * BREATHE_SPEED_MULTIPLIER;

        float x = 0;
        while (x <= (Mathf.PI * 2)) {
            breatheOffset = Mathf.Sin(x) * INTENSITY;

            yield return null; //Waits/skips one frame

            x += Time.deltaTime * SPEED;
        }

        breatheOffset = 0;
        currentBreatheRoutine = null;
    }

    void Update() {
        if (isEnabled) {
            if (currentBreatheRoutine == null) currentBreatheRoutine = StartCoroutine(Breathe());
            if (movement.isMovingForward || movement.isMovingBackward || movement.isMovingLeft || movement.isMovingRight) {
                dragOffset = Mathf.Lerp(dragOffset, dragMax, dragTransition);

                if ((movement.isMovingForward || movement.isMovingBackward) && currentBobVerticalRoutine == null) 
                    currentBobVerticalRoutine = StartCoroutine(BobVertical());
                if ((movement.isMovingLeft) && currentBobLeftRoutine == null)
                    currentBobLeftRoutine = StartCoroutine(BobLeft());
                if ((movement.isMovingRight) && currentBobRightRoutine == null)
                    currentBobRightRoutine = StartCoroutine(BobRight());
            }
            else {
                dragOffset = Mathf.Lerp(dragOffset, 0, dragTransition);
            }

            Vector3 dragOffsetVector = new Vector3(0, dragOffset * DRAG_Y_MULTIPLIER, dragOffset * DRAG_Z_MULTIPLIER);
            Vector3 bobOffsetVector = new Vector3(bobLeftOffset + bobRightOffset, bobVerticalOffset, 0);
            Vector3 breatheOffsetVector = new Vector3(0, breatheOffset, 0);

            armsTransform.localPosition = armsInitialLocalPosition + dragOffsetVector + bobOffsetVector + breatheOffsetVector;
            cameraTransform.localPosition = cameraInitialLocalPosition + // camera bob is based on arms bob, but less
                Vector3.Scale(breatheOffsetVector, CAMERA_SCALE_BREATHE) +
                bobOffsetVector.magnitude * CAMERA_SCALE_BOB; // converting horizontal bobbing to vertical bobbing
        }
    }
}
