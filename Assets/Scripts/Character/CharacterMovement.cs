using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
  [SerializeField] private Rigidbody _rigidbody;
  [SerializeField] private Transform _bodyTransform, _cameraTransform;
  [SerializeField] private float _movementSpeed = 10f;
  private float _rotationSpeed = 3f;

  // Function that moves character
  public void Move(Vector2 movement) {
    // Temporary Velocity variable that we'll set player velocity to later (for now until comfortable with impl.)
    Vector3 tmpVelocity;

    // Debug.Log(movement);

    tmpVelocity = _bodyTransform.right * movement.x;
    tmpVelocity += _bodyTransform.forward * movement.y;

    _rigidbody.velocity = tmpVelocity * _movementSpeed;
  }


  // Function that changes where the character is looking
  public void Look(Vector2 rotation) {
    //Debug.Log(rotation);

    Vector3 playerRotation = _bodyTransform.rotation.eulerAngles;

    playerRotation.y += rotation.x * _rotationSpeed;

    _bodyTransform.rotation = Quaternion.Euler(playerRotation);
    if (_cameraTransform != null) {
      Vector3 cameraRotation = _cameraTransform.rotation.eulerAngles;
      cameraRotation.x -= rotation.y * _rotationSpeed;
      _cameraTransform.rotation = Quaternion.Euler(cameraRotation);
    }
  }

  public void LookInDirection(Vector3 direction) {
    var lookRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
    var temp = Quaternion.Slerp(_bodyTransform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
    _bodyTransform.rotation = Quaternion.Euler(0f, temp.eulerAngles.y, 0f);
  }
}
