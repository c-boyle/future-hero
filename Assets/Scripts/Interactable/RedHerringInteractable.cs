using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RedHerringInteractable : Interactable
{
    [Tooltip("Set to none if no item is required to red herring.")][SerializeField] private Item requireRedHerring = null;
    public UnityEvent redHerringAction;
    public string redHerringPromptText = "interact";

    protected override void OnInteract(ItemHolder itemHolder = null) {
        if (itemHolder != null && requireRedHerring == itemHolder.HeldItem) {
            redHerringAction?.Invoke();
        } else {
            base.OnInteract(itemHolder);
        }
    }

    protected override bool MeetsItemRequirement(ItemHolder itemHolder) {
        if (itemHolder != null && requireRedHerring == itemHolder.HeldItem) {
            Prompt.text = redHerringPromptText;
            return true;
        }
        return base.MeetsItemRequirement(itemHolder);
    }
}
