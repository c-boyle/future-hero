using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleInteractable : RedHerringInteractable {
  private Vector3 originalScale;
  private float numberTimes = 6;
  [SerializeField] private GameObject futurePuddle;

  void Awake() {
    originalScale = transform.localScale;
  }

  public void DryUp(PaperTowel dryingItem) {
    if (dryingItem.Use()) Shrink();
  }

  protected override bool MeetsRedHerringRequirement(ItemHolder itemHolder) {
    return base.MeetsRedHerringRequirement(itemHolder) && (itemHolder.HeldItem as PaperTowel).HasUsesLeft();
  }

  private void Shrink() {
    float xDiff = originalScale.x / (numberTimes + 1);
    float zDiff = originalScale.z / (numberTimes + 1);

    float newX = (transform.localScale.x <= xDiff) ? 0 : transform.localScale.x - xDiff;
    float newZ = (transform.localScale.z <= zDiff) ? 0 : transform.localScale.z - zDiff;

    if (newX == 0 || newZ == 0) {
      interactionAction?.Invoke();
    } else {
      transform.localScale = new Vector3(newX, transform.localScale.y, newZ);
      futurePuddle.transform.localScale = transform.localScale;
    }
  }
}
