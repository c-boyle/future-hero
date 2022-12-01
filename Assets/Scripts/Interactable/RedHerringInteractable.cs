using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RedHerringInteractable : Interactable
{
    [Tooltip("Set to none if no item is required to red herring.")][SerializeField] private Item requireRedHerring = null;
    public UnityEvent redHerringAction;
    public string redHerringPromptText = "interact";

    protected override void OnInteract(ItemHolder itemHolder = null, bool grab = false) {
        if (!grab && itemHolder != null && requireRedHerring == itemHolder.HeldItem) {
            redHerringAction?.Invoke();
        } else {
            base.OnInteract(itemHolder, grab);
        }
    }

    public override bool MeetsItemRequirement(ItemHolder itemHolder) {
        if (itemHolder != null && requireRedHerring == itemHolder.HeldItem) {
            return true;
        }
        return base.MeetsItemRequirement(itemHolder);
    }
}
