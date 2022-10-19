using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour {
  [SerializeField] private Transform handTransform;
  [SerializeField] private Item _heldItem;
  [SerializeField] private Collider holderCollider; 
  [SerializeField] private AudioSource grabAudio;

  public Item HeldItem { get => _heldItem; }

  private Transform oldParent = null;

  private void IgnoreCollisions(Item item, bool ignore) {
    foreach (Collider collider in item.GetComponentsInChildren<Collider>()) {
      Physics.IgnoreCollision(holderCollider, collider, ignore);
    }
  }

  public void GrabItem(Item itemToGrab) {
    DropItem();

    oldParent = itemToGrab.transform.parent;

    var oldGlobalScale = itemToGrab.transform.lossyScale;
    itemToGrab.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
  
    itemToGrab.transform.SetParent(handTransform, false);
    itemToGrab.transform.localPosition = new Vector3(0,0,0);
    itemToGrab.transform.Rotate(Vector3.forward, 270);

    var itemScale = itemToGrab.transform.localScale;
    var newGlobalScale = itemToGrab.transform.lossyScale;

    // Revert the size of the item picked to what it was before being picked up
    itemToGrab.transform.localScale = new(itemScale.x * oldGlobalScale.x / newGlobalScale.x, 
                                          itemScale.y * oldGlobalScale.y / newGlobalScale.y, 
                                          itemScale.z * oldGlobalScale.z / newGlobalScale.z);

    _heldItem = itemToGrab;

    if (_heldItem.Rigidbody != null) {
      _heldItem.Rigidbody.isKinematic = true;
    }
    
    IgnoreCollisions(_heldItem, true);
    grabAudio.Play();
  }

  public void DropItem() {
    if (_heldItem != null) {
      _heldItem.transform.SetParent(oldParent, true);
      if (_heldItem.Rigidbody != null) {
        _heldItem.Rigidbody.isKinematic = false;
      }

      IgnoreCollisions(_heldItem, false);

      _heldItem = null;
    }
  }
}
