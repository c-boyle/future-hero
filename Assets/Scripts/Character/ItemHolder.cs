using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour {
  [SerializeField] private Transform handTransform;
  [SerializeField] private Item _heldItem;

  public Item HeldItem { get => _heldItem; }

  private Transform oldParent = null;

  public void GrabItem(Item itemToGrab) {
    DropItem();

    oldParent = itemToGrab.transform.parent;
    var handParent = handTransform.parent;

    var oldGlobalScale = itemToGrab.transform.lossyScale;
    itemToGrab.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
  
    itemToGrab.transform.SetParent(handParent, false);
    itemToGrab.transform.localPosition = handTransform.localPosition;
    // itemToGrab.transform.rotation = oldRotation;
    itemToGrab.transform.Rotate(Vector3.right, 180);

    var itemScale = itemToGrab.transform.localScale;
    var newGlobalScale = itemToGrab.transform.lossyScale;

    // Revert the size of the item picked to what it was before being picked up
    itemToGrab.transform.localScale = new(itemScale.x * oldGlobalScale.x / newGlobalScale.x, 
                                          itemScale.y * oldGlobalScale.y / newGlobalScale.y, 
                                          itemScale.z * oldGlobalScale.z / newGlobalScale.z);

    if (itemToGrab.Rigidbody != null) {
      itemToGrab.Rigidbody.isKinematic = true;
    }

    _heldItem = itemToGrab;
  }

  public void DropItem() {
    if (_heldItem != null) {
      _heldItem.transform.SetParent(oldParent, true);
      if (_heldItem.Rigidbody != null) {
        _heldItem.Rigidbody.isKinematic = false;
      }

      _heldItem = null;
    }
  }
}
