using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePressurePlate : MonoBehaviour {
  [SerializeField] private UnityEvent enteredEvent;
  [SerializeField] private UnityEvent exitedEvent;
  [SerializeField] private bool destroyTargetOnCollision = false;
  [SerializeField] private bool disablePressurePlateOnActivation = false;

  void OnTriggerEnter(Collider collider) {
    if (CheckCollider(collider)) {
      enteredEvent?.Invoke();
      if (destroyTargetOnCollision) {
        Destroy(collider.attachedRigidbody.gameObject);
      }
    }
  }

  void OnTriggerExit(Collider collider) {
    if (CheckCollider(collider)) {
      exitedEvent?.Invoke();
      if (disablePressurePlateOnActivation) {
        Destroy(gameObject);
      }
    }
  }

  void OnCollisionEnter(Collision collision) {
    if (CheckCollider(collision.collider)) {
      enteredEvent?.Invoke();
      if (destroyTargetOnCollision) {
        Destroy(collision.rigidbody.gameObject);
      }
    }
  }
  void OnCollisionExit(Collision collision) {
    if (CheckCollider(collision.collider)) {
      exitedEvent?.Invoke();
    }
  }

  protected abstract bool CheckCollider(Collider collider);
}
