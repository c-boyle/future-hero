using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePressurePlate : MonoBehaviour {
  [SerializeField] private UnityEvent enteredEvent;
  [SerializeField] private UnityEvent exitedEvent;

  void OnTriggerEnter(Collider collider) {
    if (CheckCollider(collider)) {
      enteredEvent?.Invoke();
    }
  }

  void OnTriggerExit(Collider collider) {
    if (CheckCollider(collider)) {
      exitedEvent?.Invoke();
    }
  }

  void OnCollisionEnter(Collision collision) {
    if (CheckCollider(collision.collider)) {
      enteredEvent?.Invoke();
    }
  }
  void OnCollisionExit(Collision collision) {
    if (CheckCollider(collision.collider)) {
      exitedEvent?.Invoke();
    }
  }

  protected abstract bool CheckCollider(Collider collider);
}
