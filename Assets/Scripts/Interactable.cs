using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {
  [SerializeField] private UnityEvent interactionAction;
  [Tooltip("Set to none if no item is required to interact.")] [SerializeField] private Item requireItem = null;
  [SerializeField] private Item giveItem = null;
  [SerializeField] private bool disableAfterFirstUse = false;

  private bool firstUse = true;

  private void Start() {
    PlayerInput.PlayerInteraction += OnInteract;
  }

  private void OnInteract(object sender, InteractionEventArgs e) {
    if (!gameObject.activeSelf || (disableAfterFirstUse && !firstUse)) {
      return;
    }
    bool meetsItemRequirement = requireItem == null || (e.ItemHolder != null && requireItem == e.ItemHolder.HeldItem);
    if (Vector3.Distance(e.InteractorPosition, transform.position) <= 50f && meetsItemRequirement) {
      if (giveItem != null && e.ItemHolder != null) {
        e.ItemHolder.GrabItem(giveItem);
      }
      interactionAction?.Invoke();
      firstUse = !firstUse;
      Debug.Log("interacted");
    }
  }

  public class InteractionEventArgs : EventArgs {
    public Vector3 InteractorPosition { get; set; }
    public ItemHolder ItemHolder { get; set; } = null;
  }
}
