using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheFuture : MonoBehaviour {
  [SerializeField] private List<TimeToggle> timeToggles = new();

  private float secondsUntilTick = -1f;
  private const float updateTickSeconds = 0.2f;

  private void Update() {
    if (secondsUntilTick <= 0) {
      secondsUntilTick = updateTickSeconds;
      foreach (var timeToggle in timeToggles) {
        if (timeToggle.gameObject.activeSelf) {
          return;
        }
      }
      // If all time toggles have been disabled, then that's a win
      LevelTimer.Instance.EndLevel(true);
    }
    secondsUntilTick -= Time.deltaTime;
  }

  public void SetEnabled(bool enabled) {
    foreach (var timeToggle in timeToggles) {
      timeToggle.SetEnabled(enabled);
    }
  }
}
