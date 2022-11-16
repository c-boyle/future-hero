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
  
  private bool holding = false;

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

  private void ForceToHand() {
    if (_heldItem != null && _heldItem.Rigidbody != null) {

      Vector3 direction;

      _heldItem.Rigidbody.velocity *= pullDecay;
      Vector3 targetPosition = handTransform.position + (Vector3.up * _heldItem.itemBounds.extents.y/2);
      if (Vector3.Distance(targetPosition, _heldItem.Rigidbody.position) > pullDistance) {
        direction = (targetPosition - _heldItem.Rigidbody.position);
        _heldItem.Rigidbody.AddForce(direction * pullForce);
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
        direction = Vector3.Scale((target - modAngles), new Vector3(1,0,1));
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
      holding = true;
      _heldItem.Rigidbody.useGravity = false;
      _heldItem.SetMass(0);
      ForceToHand();
    }
    
    IgnoreCollisions(_heldItem, true);
    grabAudio.Play();
    _heldItem.PickedUp();
  }

  public void DropItem() {
    if (_heldItem != null) {
      _heldItem.ReturnToOriginal();

      if (_heldItem.Rigidbody != null) {
        holding = false;
        _heldItem.Rigidbody.useGravity = true;
      }
      IgnoreCollisions(_heldItem, false);

      _heldItem.Dropped();
      _heldItem = null;
    }
  }
}
