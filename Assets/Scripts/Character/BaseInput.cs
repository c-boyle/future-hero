using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInput : MonoBehaviour {
  [SerializeField] protected CharacterMovement movement;
  [SerializeField] private ItemHolder itemHolder;

  public static event EventHandler<Interactable.InteractionEventArgs> Interaction;

  protected void OnInteract() {
    Interaction?.Invoke(this, new Interactable.InteractionEventArgs() { InteractorPosition = transform.position, ItemHolder = itemHolder });
  }

  protected void OnDropItem() {
    itemHolder.DropItem();
  }
}
