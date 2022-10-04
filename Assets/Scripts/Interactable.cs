using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {
  [SerializeField] private UnityEvent interactionAction;
  [Tooltip("Set to none if no item is required to interact.")][SerializeField] private Item requireItem = null;
  [SerializeField] private bool destroyRequiredItemOnInteraction = false;
  [SerializeField] private Item giveItem = null;
  [SerializeField] private bool disableAfterFirstUse = false;

  private bool firstUse = true;

  private readonly static HashSet<Interactable> enabledInteractables = new();

  private const float maxInteractionRange = 15f;

  private void OnEnable() {
    enabledInteractables.Add(this);
    if (enabledInteractables.Count == 1) {
      BaseInput.Interaction += UseClosestInteractable;
    }
  }

  private void OnDisable() {
    enabledInteractables.Remove(this);
    if (enabledInteractables.Count == 0) {
      BaseInput.Interaction -= UseClosestInteractable;
    }
  }

  private void OnInteract(object sender, InteractionEventArgs e) {

    if (!gameObject.activeSelf || (disableAfterFirstUse && !firstUse)) {
      return;
    }
    bool meetsItemRequirement = requireItem == null || (e.ItemHolder != null && requireItem == e.ItemHolder.HeldItem);
    if (meetsItemRequirement) {
      if (giveItem != null && e.ItemHolder != null) {
        e.ItemHolder.GrabItem(giveItem);
      }
      if (destroyRequiredItemOnInteraction && meetsItemRequirement) {
        var itemToDestroy = e.ItemHolder.HeldItem;
        e.ItemHolder.DropItem();
        Destroy(itemToDestroy.gameObject);
      }
      interactionAction?.Invoke();
      firstUse = !firstUse;
      Debug.Log(name + " interacted");
    }
  }

  private static void UseClosestInteractable(object sender, InteractionEventArgs e) {
    float closestInteractableDist = Mathf.Infinity;
    Interactable closestInteractable = null;
    foreach (var interactable in enabledInteractables) {
      bool interactorIsHoldingThisInteractable = e.ItemHolder != null && e.ItemHolder.HeldItem != null && e.ItemHolder.HeldItem.Interactable == interactable;
      if (!interactorIsHoldingThisInteractable) { // Skip an interactable if it's being held by the interactor
        float dist = Vector3.Distance(e.InteractorPosition, interactable.transform.position);
        if (dist < closestInteractableDist) {
          closestInteractableDist = dist;
          closestInteractable = interactable;
        }
      }
    }
    if (closestInteractableDist <= maxInteractionRange && closestInteractable != null) {
      closestInteractable.OnInteract(sender, e);
    }
  }

  public class InteractionEventArgs : EventArgs {
    public Vector3 InteractorPosition { get; set; }
    public ItemHolder ItemHolder { get; set; } = null;
  }
}
