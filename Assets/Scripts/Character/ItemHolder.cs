using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour {
  [SerializeField] private Transform handTransform;
  [SerializeField] private Item _heldItem;
  [SerializeField] private Collider holderCollider;
  [SerializeField] private AudioSource grabAudio;

  [SerializeField] private float pullForce = 8e-05f;
  [SerializeField] private float rotateForce = 0.008f;

  [SerializeField] private float pullDecay = 0.0f;
  [SerializeField] private float rotateDecay = 0.4f;

  [SerializeField] private float pullDistance = 0.0f;
  [SerializeField] private float rotateDistance = 10;

  private float maxStrength = 500f;
  private float throwMultiplier = 220f;
  private float originalItemMass;

  public bool holding = false;

  public Item HeldItem { get => _heldItem; }

  private Transform oldParent = null;

  void FixedUpdate() {
    if (holding) {
      ForceToHand();
    }
  }

  private void IgnoreCollisions(Item item, bool ignore) {
    foreach (Collider collider in item.allColliders) {
      Physics.IgnoreCollision(holderCollider, collider, ignore);
    }
  }

  private void ApplyForceToObject(Vector3 force, Item item) {
    item.Rigidbody.AddForce(force);
  }

  private void ForceToHand() {
    if (_heldItem != null && _heldItem.Rigidbody != null) {

      Vector3 direction;

      _heldItem.Rigidbody.velocity *= pullDecay;
      Vector3 targetPosition = handTransform.position + (Vector3.up * _heldItem.itemBounds.extents.y / 2);
      if (Vector3.Distance(targetPosition, _heldItem.Rigidbody.position) > pullDistance) {
        direction = (targetPosition - _heldItem.Rigidbody.position);
        ApplyForceToObject(direction * pullForce, _heldItem);
      }


      _heldItem.Rigidbody.angularVelocity *= rotateDecay;
      Vector3 angles = _heldItem.Rigidbody.rotation.eulerAngles;
      Vector3 modAngles = new Vector3(angles.x % 360, angles.y % 360, angles.z % 360);
      if ((Mathf.Abs(modAngles.x) > rotateDistance && Mathf.Abs(modAngles.x) < 360 - rotateDistance) ||
          (Mathf.Abs(modAngles.z) > rotateDistance && Mathf.Abs(modAngles.z) < 360 - rotateDistance)) {
        Debug.Log(modAngles);

        Vector3 target;
        float targetX = 0, targetZ = 0;
        if (modAngles.x > 180f) targetX = 360f;
        if (modAngles.z > 180f) targetZ = 360f;

        target = new Vector3(targetX, 0, targetZ);
        direction = Vector3.Scale((target - modAngles), new Vector3(1, 0, 1));
        _heldItem.Rigidbody.AddTorque(direction * rotateForce);
      }

    }
  }

  public void GrabItem(Item itemToGrab) {
    DropItem();

    Transform itemTransform = itemToGrab.transform;

    oldParent = itemTransform.parent;

    _heldItem = itemToGrab;

    if (_heldItem.Rigidbody != null) {
      if (_heldItem.Rigidbody.isKinematic) { // Not meant to be picked up
        _heldItem = null;
        return;
      }
      holding = true;
      _heldItem.Rigidbody.useGravity = false;
      originalItemMass = _heldItem.Rigidbody.mass;
      _heldItem.SetMass(0);
      ForceToHand();
    } else { // Not meant to be picked up
      _heldItem = null;
      return;
    }

    IgnoreCollisions(_heldItem, true);
    grabAudio.Play();
    _heldItem.PickedUp();
  }

  public void DropItem(float windup = 0) {
    if (_heldItem != null) {
      _heldItem.ReturnToOriginal();
      IgnoreCollisions(_heldItem, false);

      if (_heldItem.Rigidbody != null) {
        holding = false;
        _heldItem.Rigidbody.useGravity = true;
        _heldItem.Rigidbody.isKinematic = false;

        float outwardForce = Mathf.Pow(windup, 2) * throwMultiplier;
        Vector3 throwStrength = Vector3.ClampMagnitude(handTransform.up * outwardForce, maxStrength);
        // Debug.Log("throwStrength:" + throwStrength);
        ApplyForceToObject(throwStrength, _heldItem);
      }


      _heldItem.Dropped();
      _heldItem = null;
    }
  }

  // Code inspired by: https://www.patrykgalach.com/2020/03/23/drawing-ballistic-trajectory-in-unity/
  public void DrawThrowTrajectory(LineRenderer lineRenderer, float windup = 0) {
    if (_heldItem != null) {
      float maxCurveLength = 15f; 
      float trajectoryVertDist = 0.1f;
      float outwardForce = Mathf.Pow(windup, 2) * throwMultiplier;
      Vector3 throwStrength = Vector3.ClampMagnitude(handTransform.up * outwardForce, maxStrength);
      Vector3 velocity = (throwStrength / originalItemMass) * Time.fixedDeltaTime;
      if (velocity.magnitude == 0f) {
        return;
      }
      var startPosition = _heldItem.Rigidbody.position;

      // Create a list of trajectory points
      var curvePoints = new List<Vector3> {
        startPosition
      };
      // Initial values for trajectory
      var currentPosition = startPosition;
      var currentVelocity = velocity;
      // Init physics variables
      RaycastHit hit;
      Ray ray = new Ray(currentPosition, currentVelocity.normalized);
      
      // Loop until hit something or distance is too great
      while (!Physics.SphereCast(ray, lineRenderer.startWidth, out hit, trajectoryVertDist) && Vector3.Distance(startPosition, currentPosition) < maxCurveLength) {
        // Time to travel distance of trajectoryVertDist
        var t = trajectoryVertDist / currentVelocity.magnitude;
        // Update position and velocity
        currentVelocity += t * Physics.gravity;
        currentPosition += t * currentVelocity;
        // Add point to the trajectory
        curvePoints.Add(currentPosition);
        // Create new ray
        ray = new Ray(currentPosition, currentVelocity.normalized);
      }
      // If something was hit, add last point there
      if (hit.transform) {
        curvePoints.Add(hit.point);
      }
      // Display line with all points
      lineRenderer.positionCount = curvePoints.Count;
      lineRenderer.SetPositions(curvePoints.ToArray());
    }
  }

  public void ClearThrowTrajectory(LineRenderer lineRenderer) {
    lineRenderer.positionCount = 0;
  }
}
