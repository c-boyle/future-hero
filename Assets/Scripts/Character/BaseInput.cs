using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInput : MonoBehaviour {
  [SerializeField] protected CharacterMovement controller;
  [SerializeField] protected ItemHolder itemHolder;

  protected virtual void OnInteract() {
    Interactable.UseClosestInteractable(transform.position, itemHolder);
  }

  protected virtual void OnDropItem() {
    itemHolder.DropItem();
  }

}
