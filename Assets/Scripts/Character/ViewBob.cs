using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using System;

// TODO: Add randomness
// TODO: add jump drift

public class ViewBob : MonoBehaviour {
    [SerializeField] [MustBeAssigned] private FPSArmsManager FPSArmsManager;
    [SerializeField] [MustBeAssigned] private CharacterMovement movement;
    [SerializeField] [MustBeAssigned] private Transform armsTransform;
    [SerializeField] [MustBeAssigned] private Transform cameraTransform;

    private Vector3 cameraInitialLocalPosition;
    private Vector3 armsInitialLocalPosition;
    private Vector3 armsInitialLocalRotation;
    public float globalSpeedMultiplier; // bob speed multiplier for sprinting

    private Enumeratable bobVertical;
    private Enumeratable bobLeft;
    private Enumeratable bobRight;
    private Enumeratable breathe;
    private Enumeratable jumpLand;
    private float dragProgress = 0;
    private float yawRotation = 0;

    // Constants related to bobbing
    private const float BOB_VERTICAL_INTENSITY = 0.02f;
    private const float BOB_VERTICAL_SPEED = 6f;
    private const float BOB_VERTICAL_CYCLE = Mathf.PI;
    private readonly Func<float, float> BOB_VERTICAL_AMPLITUDE = (float x) => -Mathf.Abs(Mathf.Sin(x));

    private const float BOB_LEFT_INTENSITY = 0.02f;
    private const float BOB_LEFT_SPEED = 6f;
    private const float BOB_LEFT_CYCLE = Mathf.PI;
    private readonly Func<float, float> BOB_LEFT_AMPLITUDE = (float x) => -Mathf.Abs(Mathf.Sin(x));

    private const float BOB_RIGHT_INTENSITY = 0.02f;
    private const float BOB_RIGHT_SPEED = 6f;
    private const float BOB_RIGHT_CYCLE = Mathf.PI;
    private readonly Func<float, float> BOB_RIGHT_AMPLITUDE = (float x) => -Mathf.Abs(Mathf.Sin(x));

    private const float BREATHE_INTENSITY = 0.008f;
    private const float BREATHE_SPEED = 2f;
    private const float BREATHE_CYCLE = Mathf.PI * 2;
    private readonly Func<float, float> BREATHE_AMPLITUDE = (float x) => Mathf.Sin(x);

    private Vector3 CAMERA_BOB_MULTIPLIER = new Vector3(0, 0.05f, 0);

    // Constants related to dragging
    private const float DRAG_Z_MULTIPLIER = -0.04f;
    private const float DRAG_Y_MULTIPLIER = -0.015f;
    private float DRAG_TRANSITION = 0.08f; // how fast the arm pulls back when you begin moving

    // Constants related to rotating
    float YAW_MAX = 3f;
    float ROTATE_SIDES_TRANSITION = 0.03f;
    float ROTATE_MIDDLE_TRANSITION = 0.02f;

    // Constants for the rebound when you land upon finishing your jump
    private const float JUMP_LAND_INTENSITY = 0.0333f;
    private const float JUMP_LAND_SPEED = 16.67f;
    private const float JUMP_LAND_CYCLE = Mathf.PI;
    private readonly Func<float, float> JUMP_LAND_AMPLITUDE = (float x) => -Mathf.Abs(Mathf.Sin(x));

    // FPS arms move upwards when you begin your jump
    private const float JUMP_AIR_TRANSITION = 0.06f;
    private const float JUMP_AIR_MULTIPLIER = 0.03f;
    private float jumpAirProgress = 0;

    IEnumerator ChangeFloat(Enumeratable e, Boolean isGlobalMultiplierUsed) {
        float x = 0;
        while (x <= e.CYCLE) {
            e.currOffset = e.AMPLITUDE(x) * e.INTENSITY;

            yield return null; //Waits/skips one frame

            x += Time.deltaTime * e.SPEED * (isGlobalMultiplierUsed ? globalSpeedMultiplier : 1);
        }

        e.currOffset = 0;
        e.currCoroutine = null;
    }

    public void StartRoutine(Enumeratable e, Boolean isGlobalMultiplierUsed) {
        if (e.currCoroutine == null) e.currCoroutine = StartCoroutine(ChangeFloat(e, isGlobalMultiplierUsed));
    }

    void Start() {
        cameraInitialLocalPosition = cameraTransform.localPosition;
        armsInitialLocalPosition = armsTransform.localPosition;
        armsInitialLocalRotation = armsTransform.localRotation.eulerAngles;

        bobVertical = new Enumeratable(BOB_VERTICAL_INTENSITY, BOB_VERTICAL_SPEED, BOB_VERTICAL_CYCLE, BOB_VERTICAL_AMPLITUDE);
        bobLeft = new Enumeratable(BOB_LEFT_INTENSITY, BOB_LEFT_SPEED, BOB_LEFT_CYCLE, BOB_LEFT_AMPLITUDE);
        bobRight = new Enumeratable(BOB_RIGHT_INTENSITY, BOB_RIGHT_SPEED, BOB_RIGHT_CYCLE, BOB_RIGHT_AMPLITUDE);
        breathe = new Enumeratable(BREATHE_INTENSITY, BREATHE_SPEED, BREATHE_CYCLE, BREATHE_AMPLITUDE);
        jumpLand = new Enumeratable(JUMP_LAND_INTENSITY, JUMP_LAND_SPEED, JUMP_LAND_CYCLE, JUMP_LAND_AMPLITUDE);

        movement.onJumpStart += () => { FPSArmsManager.StartJump(); FPSArmsManager.isMidAir = true; };
        movement.onJumpLand += () => { StartRoutine(jumpLand, false); FPSArmsManager.isMidAir = false;  };
    }

    void HandleAnimations() {
        if (movement.isSprinting && movement.isGrounded) FPSArmsManager.isSprinting = true;
        else FPSArmsManager.isSprinting = false;
    }

    Vector3 HandleBob() {
        StartRoutine(breathe, false);

        if (movement.isGrounded) {
            if (movement.isMovingForward || movement.isMovingBackward)
                StartRoutine(bobVertical, true);
            if ((movement.isMovingLeft))
                StartRoutine(bobLeft, true);
            if ((movement.isMovingRight))
                StartRoutine(bobRight, true);
        }

        Vector3 breatheOffsetVector = new Vector3(0, breathe.currOffset, 0);
        Vector3 bobOffsetVector = new Vector3(bobLeft.currOffset - bobRight.currOffset, bobVertical.currOffset, 0);
        return breatheOffsetVector + bobOffsetVector;
    }

    Vector3 HandleDrag() {
        if (movement.isMovingForward || movement.isMovingBackward || movement.isMovingLeft || movement.isMovingRight)
            dragProgress = Mathf.Lerp(dragProgress, 1, DRAG_TRANSITION);
        else
            dragProgress = Mathf.Lerp(dragProgress, 0, DRAG_TRANSITION);

        Vector3 offset = new Vector3(0, dragProgress * DRAG_Y_MULTIPLIER, dragProgress * DRAG_Z_MULTIPLIER);
        return offset;
    }

    Vector3 HandleRotation() {
        if (movement.isRotatingLeft) yawRotation = Mathf.Lerp(yawRotation, YAW_MAX, ROTATE_SIDES_TRANSITION);
        else if (movement.isRotatingRight) yawRotation = Mathf.Lerp(yawRotation, -YAW_MAX, ROTATE_SIDES_TRANSITION);
        else if (!movement.isRotatingLeft && !movement.isRotatingRight) yawRotation = Mathf.Lerp(yawRotation, 0, ROTATE_MIDDLE_TRANSITION);

        return new Vector3(0, yawRotation, 0);
    }

    Vector3 HandleJumpAir() {
        if (movement.isJumpStarted)
            jumpAirProgress = Mathf.Lerp(jumpAirProgress, 1, JUMP_AIR_TRANSITION);
        else
            jumpAirProgress = Mathf.Lerp(jumpAirProgress, 0, JUMP_AIR_TRANSITION);

        Vector3 jumpAirOffset = new Vector3(0, jumpAirProgress * JUMP_AIR_MULTIPLIER, 0);
        return jumpAirOffset;
    }

    Vector3 HandleJumpLand() {
        Vector3 jumpLandOffset = new Vector3(0, jumpLand.currOffset, 0);
        return jumpLandOffset;
    }

    void Update() {

        Vector3 bobOffset = HandleBob();
        Vector3 dragOffset = HandleDrag();
        Vector3 jumpAirOffset = HandleJumpAir();
        Vector3 jumpLandOffset = HandleJumpLand();

        armsTransform.localPosition = armsInitialLocalPosition + bobOffset + dragOffset + jumpAirOffset + jumpLandOffset;
        armsTransform.localRotation = Quaternion.Euler(armsInitialLocalRotation + HandleRotation());
        cameraTransform.localPosition = cameraInitialLocalPosition + Vector3.Scale(bobOffset + jumpLandOffset, CAMERA_BOB_MULTIPLIER);

        HandleAnimations();
    }
}

public class Enumeratable {
    public float INTENSITY;
    public float SPEED;
    public float CYCLE;
    public Func<float, float> AMPLITUDE;
    public float currOffset;
    public Coroutine currCoroutine;

    public Enumeratable(float intensity, float speed, float cycle, Func<float, float> amplitude) {
        INTENSITY = intensity;
        SPEED = speed;
        CYCLE = cycle;
        AMPLITUDE = amplitude;
    }
}