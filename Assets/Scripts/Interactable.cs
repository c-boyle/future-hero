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
  }

  private void OnDisable() {
    enabledInteractables.Remove(this);
  }

  private void OnInteract(ItemHolder itemHolder) {

    if (!gameObject.activeSelf || (disableAfterFirstUse && !firstUse)) {
      return;
    }
    bool meetsItemRequirement = requireItem == null || (itemHolder != null && requireItem == itemHolder.HeldItem);
    if (meetsItemRequirement) {
      if (giveItem != null && itemHolder != null) {
        itemHolder.GrabItem(giveItem);
      }
      if (destroyRequiredItemOnInteraction && meetsItemRequirement) {
        var itemToDestroy = itemHolder.HeldItem;
        itemHolder.DropItem();
        Destroy(itemToDestroy.gameObject);
      }
      interactionAction?.Invoke();
      firstUse = !firstUse;
      // Debug.Log(name + " interacted");
    }
  }

  public static void UseClosestInteractable(Vector3 interactorPosition, ItemHolder itemHolder) {
    float closestInteractableDist = Mathf.Infinity;
    Interactable closestInteractable = null;
    foreach (var interactable in enabledInteractables) {
      bool interactorIsHoldingThisInteractable = itemHolder != null && itemHolder.HeldItem != null && itemHolder.HeldItem.Interactable == interactable;
      if (!interactorIsHoldingThisInteractable) { // Skip an interactable if it's being held by the interactor
        float dist = Vector3.Distance(interactorPosition, interactable.transform.position);
        if (dist < closestInteractableDist) {
          closestInteractableDist = dist;
          closestInteractable = interactable;
        }
      }
    }
    if (closestInteractableDist <= maxInteractionRange && closestInteractable != null) {
      closestInteractable.OnInteract(itemHolder);
    }
  }
}
