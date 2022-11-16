using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using MyBox;

public class LevelTimer : Singleton<LevelTimer> {
  [field: SerializeField] public float SecondsLeft { get; private set; } = 60f;

  public static event EventHandler<LevelEndEventArgs> LevelEnd;

  public static event EventHandler<TimerUpdateEventArgs> TimerUpdated;

  [SerializeField] public UnityEvent futureIsStarting;

  private bool levelEnded = false;
  private float originalSecondsLeft = -1f;
  private float setSecondsLeft;

  private bool futureNotTriggered = true;

  public static float SecondsSpentInLevel { get; private set; } = 0f;

  // Update is called once per frame
  void Update() {
    if (!levelEnded) {
      float deltaTime = Time.deltaTime;
      SecondsLeft -= deltaTime;
      SecondsSpentInLevel += deltaTime;
      if (SecondsLeft <= 0) {
        EndLevel(false);
      } else if (GetRealSeconds() <= 10 && futureNotTriggered) {
        futureIsStarting?.Invoke();
        futureNotTriggered = false;
      }

      TimerUpdated?.Invoke(this, new() { DeltaTime = deltaTime, SecondsLeft = SecondsLeft });
    }
  }

  private float GetRealSeconds() {
    if (originalSecondsLeft == -1f) return SecondsLeft;
    return originalSecondsLeft - (setSecondsLeft - SecondsLeft);
  }

  public void LowerSecondsLeftTo(float seconds) {
    originalSecondsLeft = SecondsLeft;
    SecondsLeft = Mathf.Min(seconds, SecondsLeft);
    setSecondsLeft = SecondsLeft;
  }

  public void RestoreSecondsLeft() {
    if (originalSecondsLeft != -1f) {
      SecondsLeft = originalSecondsLeft - (setSecondsLeft - SecondsLeft);
      originalSecondsLeft = -1f;
    }
  }

  public void EndLevel(bool won) {
    if (levelEnded) return;

    // Look at the watch
    // Skip to time end
    // Stop looking at watch
    // Play end cutscene

    LevelEnd?.Invoke(this, new() { Won = won });
    levelEnded = true;
  }

  public class LevelEndEventArgs : EventArgs {
    [field: SerializeField] public bool Won { get; set; }
  }

  public class TimerUpdateEventArgs : EventArgs {
    [field: SerializeField] public float DeltaTime { get; set; }
    [field: SerializeField] public float SecondsLeft { get; set; }
  }
}
