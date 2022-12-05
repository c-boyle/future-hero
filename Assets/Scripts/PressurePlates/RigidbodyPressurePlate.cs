using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RigidbodyPressurePlate : BasePressurePlate {

  [SerializeField] private Rigidbody rb;

  protected override bool CheckCollider(Collider collider) {
    return collider.attachedRigidbody == rb;
  }
}
