using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInput : MonoBehaviour {
  [SerializeField] protected CharacterMovement movement;
  [SerializeField] protected ItemHolder itemHolder;

  protected virtual void OnInteract() {
    Interactable.UseClosestInteractable(transform.position, itemHolder);
  }

  protected virtual void OnPickDropItem(float windup = 0) {
    if (!itemHolder.holding) {
      var cameraTransform = Camera.main.transform;
      Interactable.UseClosestInteractableInView(cameraTransform.position, cameraTransform.forward, itemHolder, true);
    }
    else itemHolder.DropItem(windup);
  }

}
