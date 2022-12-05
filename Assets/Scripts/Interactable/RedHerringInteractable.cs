using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RedHerringInteractable : Interactable {
  [Tooltip("Set to none if no item is required to red herring.")][SerializeField] protected Item requireRedHerring = null;
  public UnityEvent redHerringAction;
  public string redHerringPromptText = "interact";

  protected override void OnInteract(ItemHolder itemHolder = null, bool grab = false) {
    if (!grab && MeetsRedHerringRequirement(itemHolder)) {
      redHerringAction?.Invoke();
      if (Item is Cup cup && itemHolder.HeldItem is Cigarette cig) {
        cig.SafePosition();
        cup.OwnerIsAngry();
      }
    } else {
      base.OnInteract(itemHolder, grab);
    }
  }

  protected virtual bool MeetsRedHerringRequirement(ItemHolder itemHolder) {
    return itemHolder != null && requireRedHerring == itemHolder.HeldItem;
  }

  public override bool MeetsItemRequirement(ItemHolder itemHolder) {
    return MeetsRedHerringRequirement(itemHolder) || base.MeetsItemRequirement(itemHolder);
  }
}
