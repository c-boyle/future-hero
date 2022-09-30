using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {
  [SerializeField] private UnityEvent interactionAction;

  private void Start() {
    PlayerInput.PlayerInteraction += OnInteract;
  }

  private void OnInteract(object sender, InteractionEventArgs e) {
    if (!gameObject.activeSelf) {
      return;
    }
    if (Vector3.Distance(e.InteractorPosition, transform.position) <= 50f) {
      interactionAction?.Invoke();
      Debug.Log("interacted");
    }
  }

  public class InteractionEventArgs : EventArgs {
    public Vector3 InteractorPosition { get; set; }
  }
}
