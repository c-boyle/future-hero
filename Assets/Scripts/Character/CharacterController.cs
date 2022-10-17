using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class CharacterController : MonoBehaviour {
    [SerializeField] [MustBeAssigned] private Rigidbody _rigidbody;
    [SerializeField] [MustBeAssigned] private Transform _bodyTransform;
    [SerializeField] [MustBeAssigned] private Transform _cameraTransform;
    [SerializeField] [MustBeAssigned] private Animation _watchArmAnimation;

    [SerializeField] [ReadOnly] private float pitchDegree = 0f;
    [SerializeField] [ReadOnly] private float yawDegree = 0f;

    [PositiveValueOnly] public float SENSITIVITY = 1f;  // Mouse sensitivity
    private const float GROUND_ACCELERATION = 20f; // How fast the player gains speed on ground
    private const float AIR_ACCELERATION = 10f; // How fast the player gains speed while in the air
    private const float MAX_PITCH_DEGREE = 60; // How high or low the player can raise their head
    private const float MAX_VELOCITY = 15f; // Maximum speed the player can go
    private const float FRICTION = 5f; // How quickly the player slows down
    private const float JUMP_INTENSITY = 12f; // How high the player jumps

    // Function that gets called each time move inputs are used
    public void Move(Vector2 movement) {
        Vector3 moveVector = _bodyTransform.right * movement.x + _bodyTransform.forward * movement.y;
        Vector3 deltaVelocity = moveVector * GROUND_ACCELERATION;

        Vector3 newVelocityXZ = deltaVelocity + _rigidbody.velocity;
        newVelocityXZ.y = 0;
        newVelocityXZ = Vector3.ClampMagnitude(newVelocityXZ, MAX_VELOCITY);

        _rigidbody.velocity = newVelocityXZ + new Vector3(0, _rigidbody.velocity.y, 0);
    }

    // Function that gets called each time the mouse is moved
    public void Look(Vector2 viewDirection) {
        Vector3 cameraRotation = ConvertAngle(_cameraTransform.localRotation.eulerAngles);
        Vector3 playerRotation = ConvertAngle(_bodyTransform.rotation.eulerAngles);
        setView(cameraRotation.x - viewDirection.y * SENSITIVITY, playerRotation.y + viewDirection.x * SENSITIVITY);
    }

    // Function that lets the Future Hero Jump
    public void Jump() {
        if (_rigidbody.velocity.y < 0.1) {
            _rigidbody.AddForce(_bodyTransform.up * JUMP_INTENSITY, ForceMode.Impulse);
        }
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
     * so here is this function that gets called each timestep and does things automatically.
     */
    private Vector3 AddFriction(Vector3 currentVel, float friction, bool isYAffected=false) {
        float speed = currentVel.magnitude;
        Vector3 newVel = Vector3.zero;

        // We need to slow down the speed if it is above zero
        if (speed != 0) {
            float drop = friction * Time.deltaTime * speed;
            float scale = Mathf.Max(0, speed - drop) / speed;

            newVel = Vector3.Scale(currentVel, new Vector3(scale, isYAffected ? scale : 1, scale));
        }
        
        return newVel;
    }

    // Because I'm too lazy to set friction on every material manually
    void FixedUpdate() {
        _rigidbody.velocity = AddFriction(_rigidbody.velocity, FRICTION);
    }

    // Helper function to convert a wrapped angle to a non wrapped angle (etc. 270 -> -90)
    private Vector3 ConvertAngle(Vector3 transformAngle) {
        float x = (transformAngle.x > 180) ? transformAngle.x - 360 : transformAngle.x;
        float y = (transformAngle.y > 180) ? transformAngle.y - 360 : transformAngle.y;
        float z = (transformAngle.z > 180) ? transformAngle.z - 360 : transformAngle.z;

        return new Vector3(x, y, z);
    }

    // Function to set the camera perspective directly
    public void setView(float pitch, float yaw) {
        Vector3 cameraRotation = ConvertAngle(_cameraTransform.localRotation.eulerAngles);
        pitchDegree = Mathf.Clamp(pitch, -MAX_PITCH_DEGREE, MAX_PITCH_DEGREE);
        _cameraTransform.localRotation = Quaternion.Euler(pitchDegree, cameraRotation.y, cameraRotation.z);

        Vector3 playerRotation = ConvertAngle(_bodyTransform.rotation.eulerAngles);
        yawDegree = yaw;
        _bodyTransform.rotation = Quaternion.Euler(playerRotation.x, yawDegree, playerRotation.z);
    }
}