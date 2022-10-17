using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class CharacterController : MonoBehaviour {
    [SerializeField] [MustBeAssigned] private Rigidbody _rigidbody;
    [SerializeField] [MustBeAssigned] private Transform _bodyTransform;
    [SerializeField] [MustBeAssigned] private Transform _cameraTransform;
    [SerializeField] [MustBeAssigned] private Animation _watchArmAnimation;
    [SerializeField] private float _movementSpeed = 10f;

    [SerializeField] [ReadOnly] private float pitchDegrees = 0f;
    [SerializeField] [ReadOnly] private float yawDegrees = 0f;

    // Constants
    public float sensitivity = 1f;
    private const int maxPitchDegrees = 60;
    private const float _jumpIntensity = 300f;
    

    // Function that moves character
    public void Move(Vector2 movement, bool sprint = false) {
        // Temporary Velocity variable that we'll set player velocity to later (for now until comfortable with impl.)
        Vector3 tmpVelocity;

        // Debug.Log(movement);

        tmpVelocity = _bodyTransform.right * movement.x;
        tmpVelocity += _bodyTransform.forward * movement.y;

        tmpVelocity *= _movementSpeed;

        _rigidbody.velocity = new Vector3(tmpVelocity.x, _rigidbody.velocity.y, tmpVelocity.z);
    }

    // Function that changes where the character is looking
    public void Look(Vector2 viewDirection) {
        pitchDegrees = Mathf.Clamp(pitchDegrees - viewDirection.y * sensitivity, -maxPitchDegrees, maxPitchDegrees);
        yawDegrees = yawDegrees + viewDirection.x * sensitivity;

        _cameraTransform.localRotation = Quaternion.Euler(pitchDegrees, 0f, 0f);
        _bodyTransform.rotation = Quaternion.Euler(0f, yawDegrees, 0f);

    }

    // Function that lets the Future Hero Jump
    public void Jump() {
        if (_rigidbody.velocity.y == 0) {
            _rigidbody.AddForce(_bodyTransform.up * _jumpIntensity);
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
}