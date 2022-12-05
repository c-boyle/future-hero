using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPressurePlate : BasePressurePlate {

  [SerializeField] private Item item;

  protected override bool CheckCollider(Collider collider) {
    if (collider.attachedRigidbody.TryGetComponent(out Item hitItem)) {
      return hitItem == item;
    }
    return false;
  }
}
