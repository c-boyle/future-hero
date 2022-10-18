using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheFuture : MonoBehaviour {
  [SerializeField] private List<TimeToggle> timeToggles = new();

  private void Update() {
    foreach (var timeToggle in timeToggles) {
      if (timeToggle.gameObject.activeSelf) {
        return;
      }
    }
    // If all time toggles have been disabled, then that's a win
    // TODO: move level won event to here
  }

  public void SetEnabled(bool enabled) {
    foreach (var timeToggle in timeToggles) {
      timeToggle.SetEnabled(enabled);
    }
  }
}
