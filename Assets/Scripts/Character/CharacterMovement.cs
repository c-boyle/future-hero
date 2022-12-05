using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class CharacterMovement : MonoBehaviour {
    [SerializeField] [MustBeAssigned] private Rigidbody _rigidbody;
    [SerializeField] [MustBeAssigned] private Transform _bodyTransform;
    [SerializeField] [MustBeAssigned] private Transform _cameraTransform;
    [SerializeField] [MustBeAssigned] private Collider _bodyCollider;
    [SerializeField] [MustBeAssigned] private LayerMask _groundLayer;
    [SerializeField] [MustBeAssigned] private Transform levelStart;

    [SerializeField] [ReadOnly] private float pitchDegree = 0f;
    [SerializeField] [ReadOnly] private float yawDegree = 0f;
    [SerializeField] [ReadOnly] private Vector3 velocityPrevious = Vector3.zero;
    [SerializeField] [ReadOnly] private Vector3 velocity = Vector3.zero; // For debugging purposes
    [SerializeField] [ReadOnly] private Vector3 velocityLocal = Vector3.zero;
    [SerializeField] [ReadOnly] private Vector3 moveDirection = Vector3.zero;
    [SerializeField] [ReadOnly] private Vector3 moveDirectionLocal = Vector3.zero;
    [SerializeField] [ReadOnly] private Vector2 viewDirection = Vector2.zero;
    [SerializeField] [ReadOnly] public bool isSprinting = false;
    [SerializeField] [ReadOnly] public bool isMovingForward = false;
    [SerializeField] [ReadOnly] public bool isMovingBackward = false;
    [SerializeField] [ReadOnly] public bool isMovingRight = false;
    [SerializeField] [ReadOnly] public bool isMovingLeft = false;
    [SerializeField] [ReadOnly] public bool isRotatingRight = false;
    [SerializeField] [ReadOnly] public bool isRotatingLeft = false;
    [SerializeField] [ReadOnly] public bool isGrounded = true;
    public Action onJumpStart;
    public Action onJumpLand;
    [SerializeField] [ReadOnly] public bool isJumpTriggered = false;
    [SerializeField] [ReadOnly] public bool isJumpStarted = false;

    [PositiveValueOnly] public bool isSprintEnabled = false;  // sprintSpeed when sprinting, nonSprintSpeed otherwise
    [PositiveValueOnly] public float sprintMultiplier = 1f;  // sprintSpeed when sprinting, nonSprintSpeed otherwise
    private const float MAX_PITCH_DEGREE = 60; // How high or low the player can raise their head
    private const float GROUND_MAX_VELOCITY = 3f; // Maximum speed the player can reach while moving on ground
    private const float AIR_MAX_VELOCITY = 2.6f; // Maximum speed the player can reach while moving in midair
    private const float GROUND_ACCELERATION = 0.4f; // How fast the player gains speed on ground
    private const float AIR_ACCELERATION = 0.05f; // How fast the player gains speed while moving in midair
    private const float GROUND_FRICTION = 0.04f; // How quickly the player slows to a stop on ground
    private const float AIR_FRICTION = 0.01f; // How quickly the player slows to a stop while in midair
    private const float JUMP_INTENSITY = 4f; // How high the player jumps
    private const float GROUND_CHECK_RADIUS = 0.375f; // Radius of the collider used for ground checking
    private const float NON_STATIONARY_RATIO = 0.2f; // The player is considered "moving" if currentVelocity > MAX_VELOCITY * NON_STATIONARY_RATIO

    void Start() {
        if(!SettingsInitializer.Instance.IsTutorial) transform.position = levelStart.position + new Vector3(0, 1, 0);
    } 
    
    // Function that gets called each time move inputs are used
    public void Move(Vector2 movement) {
        moveDirection = _bodyTransform.right * movement.x + _bodyTransform.forward * movement.y;
        moveDirectionLocal = new Vector3(movement.x, 0, movement.y);
    }

    // Function that gets called each time the mouse is moved
    public void Look(Vector2 lookDirection) {
        Vector3 cameraRotation = ConvertAngle(_cameraTransform.localRotation.eulerAngles);
        Vector3 playerRotation = ConvertAngle(_bodyTransform.rotation.eulerAngles);
        var sensitivity = SettingsInitializer.Instance.Sensitiviy;
        SetView(cameraRotation.x - lookDirection.y * sensitivity, playerRotation.y + lookDirection.x * sensitivity);
        viewDirection = lookDirection;
    }

    // Function that lets the Future Hero Jump
    public void Jump() {
        if (IsGrounded()) isJumpTriggered = true;
    }

    /*
     * Usually the physics engine handles friction/drag automatically. 
     * But I'm too lazy to assign friction to every material and drag shouldn't be used,
     * so here is this function that gets called each tick and applies friction automatically.
     */
    private Vector3 AddFriction(Vector3 currentVel, float friction, bool isYAffected = false) {
        float speed = currentVel.magnitude;
        Vector3 newVel = Vector3.zero;

        // We need to slow down the speed if it is above zero
        if (speed != 0) {
            float drop = friction * speed;
            float scale = Mathf.Max(0, speed - drop) / speed;

            newVel = Vector3.Scale(currentVel, new Vector3(scale, isYAffected ? scale : 1, scale));
        }

        return newVel;
    }

    void FixedUpdate() {
        velocityPrevious = velocity;

        isGrounded = IsGrounded();
        float FRICTION = isGrounded ? GROUND_FRICTION : AIR_FRICTION;
        float ACCELERATION = (isGrounded ? GROUND_ACCELERATION : AIR_ACCELERATION) * (isSprintEnabled ? sprintMultiplier : 1);
        float MAX_VELOCITY = (isGrounded ? GROUND_MAX_VELOCITY : AIR_MAX_VELOCITY) * (isSprintEnabled ? sprintMultiplier : 1);
        Vector3 currentVelocity = _rigidbody.velocity;

        Vector3 deltaVelocity = moveDirection * ACCELERATION;
        Vector3 newVelocity = new Vector3(currentVelocity.x + deltaVelocity.x, 0, currentVelocity.z + deltaVelocity.z);
        Vector3 newVelocityClamped = Vector3.ClampMagnitude(newVelocity, MAX_VELOCITY);

        Vector3 finalVelocity = newVelocityClamped + new Vector3(0, currentVelocity.y, 0);
        Vector3 finalVelocityWithFriction = AddFriction(finalVelocity, FRICTION);
        _rigidbody.velocity = finalVelocityWithFriction;

        // For debugging purposes
        velocity = _rigidbody.velocity;
        velocityLocal = transform.InverseTransformDirection(velocity);
        isMovingForward = velocityLocal.z > MAX_VELOCITY * NON_STATIONARY_RATIO;
        isMovingBackward = -velocityLocal.z > MAX_VELOCITY * NON_STATIONARY_RATIO;
        isMovingRight = velocityLocal.x > MAX_VELOCITY * NON_STATIONARY_RATIO;
        isMovingLeft = -velocityLocal.x > MAX_VELOCITY * NON_STATIONARY_RATIO;
        isSprinting = isSprintEnabled && isMovingForward;
        isRotatingLeft = viewDirection.x < 0;
        isRotatingRight = viewDirection.x > 0;

        // Handling jump events
        if (!isJumpStarted && isJumpTriggered && isGrounded) {
            // jumped
            _rigidbody.AddForce(_bodyTransform.up * JUMP_INTENSITY, ForceMode.Impulse);
            isJumpStarted = true;
            if (onJumpStart != null) onJumpStart();
        } else if (isJumpStarted && isJumpTriggered && isGrounded) {
            // landed
            if (onJumpLand != null) onJumpLand();
            isJumpStarted = false;
            isJumpTriggered = false;
        }
    }

    // Helper function to convert a wrapped angle to a non wrapped angle (etc. 270 -> -90)
    private Vector3 ConvertAngle(Vector3 transformAngle) {
        float x = (transformAngle.x > 180) ? transformAngle.x - 360 : transformAngle.x;
        float y = (transformAngle.y > 180) ? transformAngle.y - 360 : transformAngle.y;
        float z = (transformAngle.z > 180) ? transformAngle.z - 360 : transformAngle.z;

        return new Vector3(x, y, z);
    }

    // Function to set the camera perspective directly
    public void SetView(float pitch, float yaw) {
        Vector3 cameraRotation = ConvertAngle(_cameraTransform.localRotation.eulerAngles);
        pitchDegree = Mathf.Clamp(pitch, -MAX_PITCH_DEGREE, MAX_PITCH_DEGREE);
        _cameraTransform.localRotation = Quaternion.Euler(pitchDegree, cameraRotation.y, cameraRotation.z);

        Vector3 playerRotation = ConvertAngle(_bodyTransform.rotation.eulerAngles);
        yawDegree = yaw;
        _bodyTransform.rotation = Quaternion.Euler(playerRotation.x, yawDegree, playerRotation.z);

    }

    public bool IsGrounded() {
        float offset = GROUND_CHECK_RADIUS * 0.05f;
        Vector3 groundPosition = new Vector3(_bodyCollider.bounds.center.x, _bodyCollider.bounds.min.y + GROUND_CHECK_RADIUS - offset, _bodyCollider.bounds.center.z);
        return Physics.CheckSphere(groundPosition, GROUND_CHECK_RADIUS, _groundLayer);
    }

    // Debugs ground check size
    private void OnDrawGizmos() {
        float offset = GROUND_CHECK_RADIUS * 0.05f;
        Vector3 groundPosition = new Vector3(_bodyCollider.bounds.center.x, _bodyCollider.bounds.min.y + GROUND_CHECK_RADIUS - offset, _bodyCollider.bounds.center.z);
        Gizmos.DrawWireSphere(groundPosition, GROUND_CHECK_RADIUS);
    }
}