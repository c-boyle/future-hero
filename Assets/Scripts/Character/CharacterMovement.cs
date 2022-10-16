using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
  [SerializeField] private Rigidbody _rigidbody;
  [SerializeField] private Transform _bodyTransform, _cameraTransform;
  [SerializeField] private Animation _watchArmAnimation;
  [SerializeField] private float _movementSpeed = 10f;

  private float _lookClamp = 0f;

  // Constants
  private float _rotationSpeed = 1f;
  private float _jumpIntensity = 300f;
  private int _lookHeightMax = 90;

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
  public void Look(Vector2 rotation) {

    Vector3 cameraRotation = _cameraTransform.rotation.eulerAngles;
    Vector3 playerRotation = _bodyTransform.rotation.eulerAngles;

    _lookClamp -= (rotation.y * _rotationSpeed);
    cameraRotation.x -= rotation.y * _rotationSpeed;
    playerRotation.y += rotation.x * _rotationSpeed;

    if (_lookClamp > _lookHeightMax) {
      _lookClamp = _lookHeightMax;
      cameraRotation.x = _lookClamp;
    } else if (_lookClamp < -_lookHeightMax) {
      _lookClamp = -_lookHeightMax;
      cameraRotation.x = 180 + _lookHeightMax;
    }

    _cameraTransform.rotation = Quaternion.Euler(cameraRotation);
    _bodyTransform.rotation = Quaternion.Euler(playerRotation);

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
