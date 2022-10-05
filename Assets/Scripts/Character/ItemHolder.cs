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

    itemToGrab.transform.SetParent(handParent, false);
    itemToGrab.transform.localPosition = handTransform.localPosition;
    itemToGrab.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

    var parentScale = handParent.localScale;
    var itemScale = itemToGrab.transform.localScale;
    // Adjust the item's scale by the localscale of the new parent
    itemToGrab.transform.localScale = new(itemScale.x / parentScale.x, itemScale.y / parentScale.y, itemScale.z / parentScale.z);

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
    }
  }
}
