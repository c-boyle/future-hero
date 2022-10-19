using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RigidbodyPressurePlate : MonoBehaviour {
  [SerializeField] private Rigidbody rb;
  [SerializeField] private UnityEvent enteredEvent;
  [SerializeField] private UnityEvent exitedEvent;

  void OnTriggerEnter(Collider collider) {
    if (collider.attachedRigidbody == rb) {
      enteredEvent?.Invoke();
    }
  }

  void OnTriggerExit(Collider collider) {
    if (collider.attachedRigidbody == rb) {
      exitedEvent?.Invoke();
    }
  }

  void OnCollisionEnter(Collision collision) {
    if (collision.rigidbody == rb) {
      enteredEvent?.Invoke();
    }
  }
  void OnCollisionExit(Collision collision) {
    if (collision.rigidbody == rb) {
      exitedEvent?.Invoke();
    }
  }
}
