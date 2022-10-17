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
    [SerializeField] [MustBeAssigned] private Animation _watchArmAnimation;


    [SerializeField] [ReadOnly] private float pitchDegree = 0f;
    [SerializeField] [ReadOnly] private float yawDegree = 0f;
    [SerializeField] [ReadOnly] private Vector3 velocity = Vector3.zero; // For debugging purposes
    [SerializeField] [ReadOnly] private Vector3 moveDirection = Vector3.zero;

    [PositiveValueOnly] public float sensitivity = 1f;  // Mouse sensitivity
    [PositiveValueOnly] public float sprintMultiplier = 1f;  // Increase this to a higher value when sprinting
    private const float MAX_PITCH_DEGREE = 60; // How high or low the player can raise their head
    private const float GROUND_MAX_VELOCITY = 15f; // Maximum speed the player can reach while moving on ground
    private const float AIR_MAX_VELOCITY = 11f; // Maximum speed the player can reach while moving in midair
    private const float GROUND_ACCELERATION = 3f; // How fast the player gains speed on ground
    private const float AIR_ACCELERATION = 0.05f; // How fast the player gains speed while moving in midair
    private const float GROUND_FRICTION = 0.11f; // How quickly the player slows to a stop on ground
    private const float AIR_FRICTION = 0f; // How quickly the player slows to a stop while in midair
    private const float JUMP_INTENSITY = 12f; // How high the player jumps

    // Function that gets called each time move inputs are used
    public void Move(Vector2 movement) {
        moveDirection = _bodyTransform.right * movement.x + _bodyTransform.forward * movement.y;
    }

    // Function that gets called each time the mouse is moved
    public void Look(Vector2 viewDirection) {
        Vector3 cameraRotation = ConvertAngle(_cameraTransform.localRotation.eulerAngles);
        Vector3 playerRotation = ConvertAngle(_bodyTransform.rotation.eulerAngles);
        SetView(cameraRotation.x - viewDirection.y * sensitivity, playerRotation.y + viewDirection.x * sensitivity);
    }

    // Function that lets the Future Hero Jump
    public void Jump() {
        if (IsGrounded()) _rigidbody.AddForce(_bodyTransform.up * JUMP_INTENSITY, ForceMode.Impulse);
    }

    // Function that triggers the animation to look at the watch
    public void LookAtWatch() {
        _watchArmAnimation.Stop("reverseToggleWatchArm_Left");
        // _watchArmAnimation["reverseToggleWatchArm_Left"].speed = 1f;
        _watchArmAnimation.Play("reverseToggleWatchArm_Left");
    }

    public void PutWatchAway() {
        _watchArmAnimation.Stop("toggleWatchArm_Left");
        // _watchArmAnimation["toggleWatchArm_Left"].speed = -1f;
        _watchArmAnimation.Play("toggleWatchArm_Left");
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
        bool isGrounded = IsGrounded();
        float FRICTION = isGrounded ? GROUND_FRICTION : AIR_FRICTION;
        float ACCELERATION = (isGrounded ? GROUND_ACCELERATION : AIR_ACCELERATION) * sprintMultiplier;
        float MAX_VELOCITY = (isGrounded ? GROUND_MAX_VELOCITY : AIR_MAX_VELOCITY) * sprintMultiplier;
        Vector3 currentVelocity = _rigidbody.velocity;

        Vector3 deltaVelocity = moveDirection * ACCELERATION;
        Vector3 newVelocity = new Vector3(currentVelocity.x + deltaVelocity.x, 0, currentVelocity.z + deltaVelocity.z);
        Vector3 newVelocityClamped = Vector3.ClampMagnitude(newVelocity, MAX_VELOCITY);

        Vector3 finalVelocity = newVelocityClamped + new Vector3(0, currentVelocity.y, 0);
        Vector3 finalVelocityWithFriction = AddFriction(finalVelocity, FRICTION);
        _rigidbody.velocity = finalVelocityWithFriction;

        // For debugging purposes
        velocity = _rigidbody.velocity;
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
        Vector3 groundPosition = new Vector3(_bodyCollider.bounds.center.x, _bodyCollider.bounds.min.y, _bodyCollider.bounds.center.z);
        return Physics.CheckSphere(groundPosition, 1f, _groundLayer);
    }
}